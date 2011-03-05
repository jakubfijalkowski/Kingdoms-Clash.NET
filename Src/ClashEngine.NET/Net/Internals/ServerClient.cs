using System.Net;
using System.Net.Sockets;
using System;

namespace ClashEngine.NET.Net.Internals
{
	using Interfaces.Net;

	/// <summary>
	/// Klient używany przez klasę serwera.
	/// </summary>
	internal class ServerClient
		: TcpClientBase
	{
		#region Statics
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		#endregion

		#region Internal fields
		internal Messages.ClientWelcome? WelcomeMessage = null;
		#endregion

		#region IClient Members
		/// <summary>
		/// Status.
		/// </summary>
		public new ClientStatus Status
		{
			get { return base.Status; }
			internal set { base.Status = value; }
		}

		/// <summary>
		/// Otwiera połączenie.
		/// </summary>
		public override void Open()
		{ }

		/// <summary>
		/// Zamyka połączenie z klientem.
		/// </summary>
		public override void Close()
		{
			if (this.Status == ClientStatus.Closed) //Wywołane po zamknięciu połączenia z zewnątrz - usuwamy z serwera
			{
				Logger.Info("Client {0}:{1} closed the connection", this.Endpoint.Address, this.Endpoint.Port);
				this.Status = (ClientStatus)0xFFFF;
			}
			else if (this.Status != (ClientStatus)0xFFFF) //Wywołane ręcznie - zamykmy i usuwamy
			{
				Logger.Info("Client {0}:{1} - connection closed", this.Endpoint.Address, this.Endpoint.Port);
				this.Send(new Message(MessageType.Close, null));
				this.Status = (ClientStatus)0xFFFF;
				this.Socket.Close();
			}
		}

		/// <summary>
		/// Przygotowuje klienta do współpracy z serwerem(wymiana podstawowych wiadomości).
		/// </summary>
		public override void Prepare()
		{
			this.Receive();
			if (this.Messages.Count > 0)
			{
				if (this.Messages[0].Type == MessageType.Welcome)
				{
					try
					{
						this.WelcomeMessage = new Messages.ClientWelcome(this.Messages[0]);
						this.Version = this.WelcomeMessage.Value.Version;
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
				else
				{
					Logger.Warn("Client {0}:{1} rejected - invalid welcome sequence", this.Endpoint.Address, this.Endpoint.Port);
					this.Status = ClientStatus.Error;
					this.Send(new Message(MessageType.InvalidSequence, null));
					this.Close();
				}
			}
		}
		#endregion

		#region Constructors
		public ServerClient(Socket socket)
			: base((IPEndPoint)socket.RemoteEndPoint)
		{
			base.Socket = socket;
			base.Socket.Blocking = false;
			this.Status = ClientStatus.Welcome;
		}
		#endregion
	}
}
