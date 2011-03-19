using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Serwer oparty na TCP.
	/// </summary>
	/// <remarks>
	/// Port informacji używa UDP.
	/// </remarks>
	public class TcpServer
		: IServer, IDisposable
	{
		#region Statics
		private const int ListenBacklog = 10;
		private static readonly TimeSpan DefaultMaxIdleTime = new TimeSpan(0, 1, 0);
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET.Server");
		#endregion

		#region Private fields
		private Internals.ServerClientsCollection _ClientsCollection = new Internals.ServerClientsCollection();
		private Thread ServerThread = null;
		private bool ServerStop = false;
		private byte[] InfoportBuffer = new byte[1024];
		#endregion

		#region IServer Members
		/// <summary>
		/// Nazwa gry.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Wersja.
		/// </summary>
		public Version Version { get; private set; }

		/// <summary>
		/// Dane, na których serwer nasłuchuje.
		/// </summary>
		public IPEndPoint Endpoint { get; private set; }

		/// <summary>
		/// Dane, na których otwarty jest kanał informacyjny.
		/// Null, jeśli taki kanał nie jest otwarty.
		/// </summary>
		public IPEndPoint InfoEndpoint { get; private set; }

		/// <summary>
		/// Maksymalna liczba klientów podłączonych do serwera.
		/// </summary>
		public uint MaxClients { get; private set; }

		/// <summary>
		/// Lista klientów aktualnie podłączonych do serwera.
		/// </summary>
		public IClientsCollection Clients
		{
			get { return this._ClientsCollection; }
		}

		/// <summary>
		/// Maksymlny czas braku aktywności ze strony klienta. Po jego przekroczeniu klient jest rozłączany.
		/// </summary>
		public TimeSpan MaxClientIdleTime { get; set; }

		/// <summary>
		/// Stan serwera.
		/// </summary>
		public ServerState State { get; private set; }

		/// <summary>
		/// Dane dodatkowe wysyłane razem z informacjami o serwerze.
		/// </summary>
		public string AdditionalData { get; set; }

		/// <summary>
		/// Startuje serwer na nowym wątku.
		/// </summary>
		/// <param name="wait">Określa, czy czekać do pełnego uruchomienia serwera.</param>
		public void Start(bool wait = true)
		{
			if (this.State == ServerState.Stopped)
			{
				this.State = ServerState.Starting;
				this.ServerThread.Start();
				while (wait && this.State == ServerState.Starting) //Blokujemy aktualny wątek do pełnego uruchomienia serwera.
					Thread.Sleep(10);
			}
		}

		/// <summary>
		/// Zatrzymuje działanie serwera rozłączając wszystkich klientów i kończąc dodatkowy wątek.
		/// </summary>
		/// <param name="wait">Określa, czy czekać do pełnego zatrzymania serwera.</param>
		public void Stop(bool wait = true)
		{
			if (this.State == ServerState.Running)
			{
				this.ServerStop = true;
				if (wait)
					this.ServerThread.Join();
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje serwer.
		/// </summary>
		/// <param name="endpoint"><see cref="Endpoint"/></param>
		/// <param name="maxClients"><see cref="MaxClients"/></param>
		/// <param name="name"><see cref="Name"/></param>
		/// <param name="version"><see cref="Version"/></param>
		public TcpServer(IPEndPoint endpoint, uint maxClients, string name, Version version, IPEndPoint infoEndpoint = null)
		{
			this.Endpoint = endpoint;
			this.InfoEndpoint = infoEndpoint;
			this.MaxClients = maxClients;
			this.Name = name;
			this.Version = version;

			this.MaxClientIdleTime = DefaultMaxIdleTime;

			this.ServerThread = new Thread(this.ServerMain);
			this.ServerThread.Name = "TcpServer: " + this.Version.ToString() + " " + this.Name;
		}

		/// <summary>
		/// Inicjalizuje serwer.
		/// </summary>
		/// <param name="port">Port, na którym serwer nasłuchuje.</param>
		/// <param name="maxClients"><see cref="MaxClients"/></param>
		/// <param name="name"><see cref="Name"/></param>
		/// <param name="version"><see cref="Version"/></param>
		/// <param name="infoPort"><see cref="InfoEndpoint"/></param>
		public TcpServer(int port, uint maxClients, string name, Version version, int infoPort = 0)
			: this(new IPEndPoint(IPAddress.Any, port), maxClients, name, version, new IPEndPoint(IPAddress.Any, infoPort))
		{ }
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Stop();
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Główna metoda serwera.
		/// </summary>
		private void ServerMain()
		{
			#region Starting
			Logger.Info("Starting TcpServer: {0} {1} at {2}:{3}", this.Name, this.Version, this.Endpoint.Address, this.Endpoint.Port);
			Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				listenSocket.Blocking = false;
				listenSocket.Bind(this.Endpoint);
				listenSocket.Listen(ListenBacklog);
			}
			catch (Exception ex)
			{
				this.State = ServerState.Error;
				Logger.ErrorException("Cannot start server", ex);
				return;
			}
			this.State = ServerState.Running;
			#endregion

			#region Information channel - opening
			Socket infoSocket = null;
			if (this.InfoEndpoint != null)
			{
				try
				{
					Logger.Info("Opening information channel at {0}:{1}", this.InfoEndpoint.Address, this.InfoEndpoint.Port);
					infoSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					infoSocket.Blocking = false;
					infoSocket.Bind(this.InfoEndpoint);
					Logger.Info("Information channel opened");
				}
				catch (Exception ex)
				{
					Logger.WarnException("Cannot open information channel", ex);
					throw;
				}
			}
			#endregion

			Logger.Info("Server started");

			#region Main loop
			while (!this.ServerStop)
			{
				bool isAnybodyToAccept = false;
				try
				{
					isAnybodyToAccept = listenSocket.Poll(0, SelectMode.SelectRead);
				}
				catch (Exception ex)
				{
					Logger.WarnException("Cannot check if there is anybody to accept", ex);
				}

				if (isAnybodyToAccept) //Mamy kogoś do dodania 
				{
					Socket socket = null;
					try
					{
						socket = listenSocket.Accept();
						this.HandleNewConnection(socket);
					}
					catch (Exception ex)
					{
						Logger.WarnException("Cannot handle new connection", ex);
						if (socket != null)
						{
							socket.Close();
						}
					}
				}
				for (int i = 0; i < this.Clients.Count; i++)
				{
					var client = this.Clients[i] as Internals.ServerClient;
					if (client.Status == ClientStatus.Welcome) //Sekwencja powitalna
					{
						this.HandleWelcomeSequence(client, ref i);
					}
					else
					{
						this.HandleClient(client);
					}
				}
				if (infoSocket != null)
				{
					this.HandleInfoport(infoSocket);
				}
			}
			#endregion

			#region Stopping
			Logger.Info("Stopping TcpServer: {0} {1} at {2}:{3}", this.Name, this.Version, this.Endpoint.Address, this.Endpoint.Port);
			if (this.InfoEndpoint != null)
			{
				infoSocket.Close();
				Logger.Info("Information channel closed");
			}

			this._ClientsCollection.Clear();
			listenSocket.Close();
			this.ServerStop = false;
			this.State = ServerState.Stopped;
			Logger.Info("Server stopped");
			#endregion
		}

		/// <summary>
		/// Obsługa nowego połączenia.
		/// </summary>
		private void HandleNewConnection(Socket socket)
		{
			var client = new Internals.ServerClient(socket);
			if (this.MaxClients > this.Clients.Count)
			{
				client.Send(new Messages.ServerWelcome(this.Version, this.Name).ToMessage());
				this._ClientsCollection.InternalAdd(client);
				Logger.Info("Client {0}:{1} accepted, starting welcome sequence", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
			}
			else
			{
				client.Send(new Message(MessageType.TooManyConnections, null));
				client.CloseSocket();
				Logger.Info("Client {0}:{1} rejected, reason: too many connections", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
			}
		}

		/// <summary>
		/// Obsługa sekwencji powitalnej.
		/// </summary>
		/// <param name="client"></param>
		/// <param name="i"></param>
		private void HandleWelcomeSequence(Internals.ServerClient client, ref int i)
		{
			client.Receive();
			if (client.Messages.Count > 0)
			{
				if (client.Messages[0].Type == MessageType.Welcome) //Odebraliśmy wiadomość powitalną
				{
					Messages.ClientWelcome msg;
					try
					{
						msg = new Messages.ClientWelcome(client.Messages[0]);
					}
					catch //Błędna
					{
						client.Send(new Message(MessageType.InvalidSequence, null));
						client.CloseSocket();
						Logger.Info("Client {0}:{1} rejected, reason: invalid welcome sequence", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
						this._ClientsCollection.RemoveAt(i--);
						return;
					}
					client.Messages.RemoveAt(0);
					if (msg.Version == this.Version) //Wszystko ok
					{
						client.Send(new Message(MessageType.AllOk, null));
						client.Status = ClientStatus.Ok;
						Logger.Info("Client {0}:{1} - welcome sequence went well", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
					}
					else //Niepoprawna wersja
					{
						client.Send(new Message(MessageType.IncompatibleVersion, null));
						client.CloseSocket();
						Logger.Info("Client {0}:{1} rejected, reason: incompatible version", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
						this._ClientsCollection.RemoveAt(i--);
					}
				}
				else if (client.Messages[0].Type == MessageType.IncompatibleVersion ||
					client.Messages[0].Type == MessageType.InvalidSequence) //Klient ma inną wersję niż my
				{
					client.CloseSocket();
					Logger.Info("Client {0}:{1} closed the connection, reason: incompatible version", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
					this._ClientsCollection.RemoveAt(i--);
				}
				else //Błędna sekwencja ze strony klienta
				{
					client.Send(new Message(MessageType.InvalidSequence, null));
					client.CloseSocket();
					Logger.Info("Client {0}:{1} rejected, reason: invalid welcome sequence", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
					this._ClientsCollection.RemoveAt(i--);
				}
			}
		}

		/// <summary>
		/// Obsługa normalnego działania.
		/// </summary>
		/// <param name="client"></param>
		private void HandleClient(Internals.ServerClient client)
		{
			if (DateTime.Now - client.LastAction > this.MaxClientIdleTime)
			{
				client.Send(new Message(MessageType.TimeOut, null));
				client.CloseSocket();
				client.Status = ClientStatus.NotResponding;
				Logger.Info("Client {0}:{1} - connection closed, client is not responding", client.RemoteEndpoint.Address, client.RemoteEndpoint.Port);
			}
			else
			{
				client.Receive();
			}
		}

		/// <summary>
		/// Obsługa kanału informacji.
		/// </summary>
		/// <param name="socket"></param>
		private void HandleInfoport(Socket socket)
		{
			if (socket.Available > 0)
			{
				EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
				try
				{
					var length = socket.ReceiveFrom(this.InfoportBuffer, ref remote);
					if (length > 0)
					{
						if (new Message(this.InfoportBuffer, 0, length).Type == MessageType.Welcome)
						{
							//Wysyłamy informacje o serwerze
							socket.SendTo(this.GetServerInfo(), remote);
						}
					}
				}
				catch
				{ }
			}
		}

		/// <summary>
		/// Formatuje dane o serwerze
		/// </summary>
		/// <returns></returns>
		private byte[] GetServerInfo()
		{
			byte[] data = new byte[
				2   //Długość
				+ 2 * this.Name.Length //Nazwa
				+ 4 //Wersja
				+ 4 //Aktualna liczba klientów
				+ 4 //Max. klientów
				+ 2 //Długość
				+ 2 * this.AdditionalData.Length //Dodatkowe dane
				];

			var nameLen     = BitConverter.GetBytes((ushort)this.Name.Length);
			var currClients = BitConverter.GetBytes((uint)this.Clients.Count);
			var maxClients  = BitConverter.GetBytes(this.MaxClients);
			var addDataLen  = BitConverter.GetBytes((ushort)this.AdditionalData.Length);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(nameLen);
				Array.Reverse(currClients);
				Array.Reverse(maxClients);
				Array.Reverse(addDataLen);
			}
			int i = 0;
			Array.Copy(nameLen, data, 2);
			i += 2;
			System.Text.Encoding.Unicode.GetBytes(this.Name, 0, this.Name.Length, data, 2);
			i += this.Name.Length * 2;
			data[i++] = (byte)this.Version.Major;
			data[i++] = (byte)this.Version.Minor;
			data[i++] = (byte)this.Version.Build;
			data[i++] = (byte)this.Version.Revision;
			Array.Copy(currClients, 0, data, i, 4); i += 4;
			Array.Copy(maxClients, 0, data, i, 4); i += 4;
			Array.Copy(addDataLen, 0, data, i, 2); i += 2;
			System.Text.Encoding.Unicode.GetBytes(this.AdditionalData, 0, this.AdditionalData.Length, data, i);
			return data;
		}
		#endregion
	}
}
