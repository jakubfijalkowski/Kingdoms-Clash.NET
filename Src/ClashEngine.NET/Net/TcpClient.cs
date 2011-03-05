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
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
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
			if (!this.Socket.Connected)
			{
				this.Status = ClientStatus.Welcome;
				Logger.Info("Opening connection to {0}:{1}", base.Endpoint.Address, base.Endpoint.Port);
				try
				{
					base.Socket.Connect(base.Endpoint);
				}
				catch (Exception ex)
				{
					base.Status = ClientStatus.Error;
					Logger.ErrorException("Cannot connecto to server", ex);
					throw;
				}
			}
		}

		/// <summary>
		/// Zamyka połączenie z serwerem.
		/// </summary>
		public override void Close()
		{
			this.Send(new Message(MessageType.Close, null));
			this.Status = ClientStatus.Closed;
			this.Socket.Close();
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
					this.Send(new Messages.ClientWelcome(this.Version).ToMessage());
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
				Logger.Warn("Connection to {0}:{1} aborted - too many conections", this.Endpoint.Address, this.Endpoint.Port);
				this.Status = ClientStatus.TooManyConnections;
				this.Socket.Close();
			}
			else if (this.Messages[0].Type == MessageType.InvalidSequence)
			{
				Logger.Warn("Connection to {0}:{1} aborted - invalid welcome sequence(from us)", this.Endpoint.Address, this.Endpoint.Port);
				this.Status = ClientStatus.InvalidSequence;
				this.Socket.Close();
			}
			else
			{
				Logger.Warn("Connection to {0}:{1} aborted - invalid welcome sequence", this.Endpoint.Address, this.Endpoint.Port);
				this.Status = ClientStatus.Error;
				this.Send(new Message(MessageType.InvalidSequence, null));
				this.Close();
			}
			this.WelcomeSent = true;
		}

		private void WelcomePhase2()
		{
			switch (this.Messages[0].Type)
			{
				case MessageType.AllOk:
					Logger.Info("Connection to {0}:{1} established", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.Ok;
					break;

				case MessageType.IncompatibleVersion:
					Logger.Warn("Connection to {0}:{1} aborted - incompatible version", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.IncompatibleVersion;
					this.Socket.Close();
					break;

				case MessageType.InvalidSequence:
					Logger.Warn("Connection to {0}:{1} aborted - invalid welcome sequence(from us)", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.InvalidSequence;
					this.Socket.Close();
					break;

				default:
					Logger.Warn("Connection to {0}:{1} aborted - invalid welcome sequence", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.Error;
					this.Send(new Message(MessageType.InvalidSequence, null));
					this.Close();
					break;
			}
			this.Messages.RemoveAt(0);
		}
		#endregion
	}
}
