using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Klient TCP.
	/// </summary>
	public class TcpClient
		: TcpClientBase, IDisposable
	{
		#region Statics
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET.Client");
		#endregion

		#region Private fields
		private bool WelcomeSent = false;
		private Thread ClientThread = null;
		private bool StopConnection = false;
		#endregion

		#region IClient Members
		/// <summary>
		/// Startuje nowy wątek obsługujący połączenie.
		/// </summary>
		/// <param name="wait">Określa, czy czekać na otwarcie połączenia i zakończenie sekwencji powitalnej.</param>
		public override void Open(bool wait = true)
		{
			if (this.Status == ClientStatus.Closed)
			{
				this.Status = ClientStatus.Welcome;
				this.ClientThread.Start();
				while (wait && this.Status == ClientStatus.Welcome)
					Thread.Sleep(10);
			}
		}

		/// <summary>
		/// Zamyka dodatkowy wątek.
		/// </summary>
		/// <param name="wait">Określa, czy czekać na zakończenie połączenia.</param>
		public override void Close(bool wait = true)
		{
			if (this.Status == ClientStatus.Ok)
			{
				this.StopConnection = true;
				if (wait)
					this.ClientThread.Join();
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje klienta.
		/// </summary>
		/// <param name="endpoint">Adres do serwera.</param>
		/// <param name="clientVersion">Wersja klienta.</param>
		public TcpClient(IPEndPoint endpoint, Version clientVersion)
			: base(endpoint)
		{
			base.Version = clientVersion;
			base.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.Status = ClientStatus.Closed;

			this.ClientThread = new Thread(ClientMain);
			this.ClientThread.Name = string.Format("TcpClient: {0}:{1}", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Close();
		}
		#endregion

		#region Protected Methods
		protected override bool HandleNewMessage(Message msg)
		{
			switch (msg.Type)
			{
				case MessageType.TooManyConnections: //Tylko podczas sekwencji powitalnej
				case MessageType.Welcome:
				case MessageType.AllOk:
				case MessageType.IncompatibleVersion:
				case MessageType.InvalidSequence:
					return this.Status == ClientStatus.Welcome;

				case MessageType.Close:
					this.Socket.Close();
					this.Status = ClientStatus.Closed;
					Logger.Info("Server have closed the connection");
					return false;

				case MessageType.TimeOut:
					this.Socket.Close();
					this.Status = ClientStatus.NotResponding;
					Logger.Error("Time out");
					return false;
			}
			return true;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Obsługuje połączenie z serwerem.
		/// </summary>
		private void ClientMain()
		{
			#region Opening connection
			Logger.Info("Opening connection to {0}:{1}", base.RemoteEndpoint.Address, base.RemoteEndpoint.Port);
			try
			{
				base.Socket.Connect(base.RemoteEndpoint);
			}
			catch (Exception ex)
			{
				base.Status = ClientStatus.Error;
				Logger.ErrorException("Cannot connect to server", ex);
				return;
			}
			Logger.Info("Connection opened");
			#endregion

			#region Welcome sequence
			while (this.Status == ClientStatus.Welcome)
			{
				base.Receive(true);
				if (this.Messages.Count > 0)
				{
					if (!this.WelcomeSent)
					{
						this.WelcomePhase1();
					}
					else
					{
						this.WelcomePhase2();
					}
				}
			}
			if (this.Status != ClientStatus.Ok) //Błąd - koniec
			{
				return;
			}
			#endregion

			#region Main code
			while (!this.StopConnection)
			{
				try
				{
					base.Receive();
					if (this.Status != ClientStatus.Ok) //Błąd - koniec
					{
						return;
					}
				}
				catch (Exception ex)
				{
					this.Socket.Close();
					this.Status = ClientStatus.Error;
					Logger.ErrorException("Error occured while retriving data", ex);
					return;
				}
			}
			#endregion

			#region Closing connection
			this.Send(new Message(MessageType.Close, null));
			this.Status = ClientStatus.Closed;
			this.Socket.Close();
			Logger.Info("Connection to {0}:{1} closed", base.RemoteEndpoint.Address, base.RemoteEndpoint.Port);
			#endregion
		}

		/// <summary>
		/// Faza pierwsza powitania - wiadomość Welcome.
		/// </summary>
		private void WelcomePhase1()
		{
			if (this.Messages[0].Type == MessageType.Welcome)
			{
				try
				{
					var msg = new Messages.ServerWelcome(this.Messages[0]);
					Logger.Info("Connected to {0}:{1} - {2} {3}", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port, msg.GameName, msg.ServerVersion);
					if (msg.ServerVersion != this.Version) //Błędna wersja serwera.
					{
						this.Send(new Message(MessageType.IncompatibleVersion, null));
						this.Socket.Close();
						this.Status = ClientStatus.IncompatibleVersion;
						Logger.Error("Server has incompatible version");
					}
					else
					{
						this.Send(new Messages.ClientWelcome(this.Version).ToMessage());
					}
				}
				catch
				{
					Logger.Warn("Client {0}:{1} rejected - invalid Welcome message", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
					this.Status = ClientStatus.Error;
					this.Send(new Message(MessageType.InvalidSequence, null));
					this.Close();
				}
				this.Messages.RemoveAt(0);
			}
			else if (this.Messages[0].Type == MessageType.TooManyConnections)
			{
				Logger.Error("Connection to {0}:{1} aborted - too many conections", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
				this.Status = ClientStatus.TooManyConnections;
				this.Socket.Close();
			}
			else if (this.Messages[0].Type == MessageType.InvalidSequence)
			{
				Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence(from us)", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
				this.Status = ClientStatus.InvalidSequence;
				this.Socket.Close();
			}
			else
			{
				Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
				this.Status = ClientStatus.Error;
				this.Send(new Message(MessageType.InvalidSequence, null));
				this.Close();
			}
			this.WelcomeSent = true;
		}

		/// <summary>
		/// Faza druga powitania - odebranie wiadomości o stanie połączenia.
		/// </summary>
		private void WelcomePhase2()
		{
			switch (this.Messages[0].Type)
			{
				case MessageType.AllOk:
					Logger.Info("Connection to {0}:{1} established", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
					this.Status = ClientStatus.Ok;
					break;

				case MessageType.IncompatibleVersion:
					Logger.Error("Connection to {0}:{1} aborted - incompatible version", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
					this.Status = ClientStatus.IncompatibleVersion;
					this.Socket.Close();
					break;

				case MessageType.InvalidSequence:
					Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence(from us)", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
					this.Status = ClientStatus.InvalidSequence;
					this.Socket.Close();
					break;

				default:
					Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
					this.Status = ClientStatus.Error;
					this.Send(new Message(MessageType.InvalidSequence, null));
					this.Socket.Close();
					break;
			}
			this.Messages.RemoveAt(0);
		}
		#endregion
	}
}
