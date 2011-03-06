using System;
using System.Net;
using System.Net.Sockets;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Klient TCP.
	/// </summary>
	public class TcpClient
		: TcpClientBase
	{
		#region Statics
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET.Client");
		#endregion

		#region Private fields
		private bool WelcomeSent = false;
		#endregion

		#region IClient Members
		/// <summary>
		/// Otwiera połączenie z serwerem.
		/// </summary>
		public override void Open()
		{
			if (!this.Socket.Connected && this.Status == ClientStatus.Closed)
			{
				Logger.Info("Opening connection to {0}:{1}", base.Endpoint.Address, base.Endpoint.Port);
				try
				{
					base.Socket.Connect(base.Endpoint);
				}
				catch (Exception ex)
				{
					base.Status = ClientStatus.Error;
					Logger.ErrorException("Cannot connect to server", ex);
					throw;
				}
				this.Status = ClientStatus.Welcome;
				Logger.Info("Connection opened");
			}
		}

		/// <summary>
		/// Zamyka połączenie z serwerem.
		/// </summary>
		public override void Close()
		{
			if (this.Status == ClientStatus.Ok)
			{
				this.Send(new Message(MessageType.Close, null));
				this.Status = ClientStatus.Closed;
				this.Socket.Close();
				Logger.Info("Connection to {0}:{1} closed", base.Endpoint.Address, base.Endpoint.Port);
			}
		}

		/// <summary>
		/// Obsługuje sekwencje powitalną.
		/// </summary>
		public override void Prepare()
		{
			if (this.Status == ClientStatus.Welcome)
			{
				base.Receive();
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
		/// Faza pierwsza powitania - wiadomość Welcome.
		/// </summary>
		private void WelcomePhase1()
		{
			if (this.Messages[0].Type == MessageType.Welcome)
			{
				try
				{
					var msg = new Messages.ServerWelcome(this.Messages[0]);
					Logger.Info("Connected to {0}:{1} - {2} {3}", this.Endpoint.Address, this.Endpoint.Port, msg.GameName, msg.ServerVersion);
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
					Logger.Warn("Client {0}:{1} rejected - invalid Welcome message", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.Error;
					this.Send(new Message(MessageType.InvalidSequence, null));
					this.Close();
				}
				this.Messages.RemoveAt(0);
			}
			else if (this.Messages[0].Type == MessageType.TooManyConnections)
			{
				Logger.Error("Connection to {0}:{1} aborted - too many conections", this.Endpoint.Address, this.Endpoint.Port);
				this.Status = ClientStatus.TooManyConnections;
				this.Socket.Close();
			}
			else if (this.Messages[0].Type == MessageType.InvalidSequence)
			{
				Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence(from us)", this.Endpoint.Address, this.Endpoint.Port);
				this.Status = ClientStatus.InvalidSequence;
				this.Socket.Close();
			}
			else
			{
				Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence", this.Endpoint.Address, this.Endpoint.Port);
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
					Logger.Info("Connection to {0}:{1} established", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.Ok;
					break;

				case MessageType.IncompatibleVersion:
					Logger.Error("Connection to {0}:{1} aborted - incompatible version", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.IncompatibleVersion;
					this.Socket.Close();
					break;

				case MessageType.InvalidSequence:
					Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence(from us)", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.InvalidSequence;
					this.Socket.Close();
					break;

				default:
					Logger.Error("Connection to {0}:{1} aborted - invalid welcome sequence", this.Endpoint.Address, this.Endpoint.Port);
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
