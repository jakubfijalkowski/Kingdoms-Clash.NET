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
		public TcpServer(IPEndPoint endpoint, uint maxClients, string name, Version version)
		{
			this.Endpoint = endpoint;
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
		public TcpServer(int port, uint maxClients, string name, Version version)
			: this(new IPEndPoint(IPAddress.Any, port), maxClients, name, version)
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
			}
			this._ClientsCollection.Clear();
			listenSocket.Close();
			this.ServerStop = false;
			this.State = ServerState.Stopped;
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
		#endregion
	}
}
