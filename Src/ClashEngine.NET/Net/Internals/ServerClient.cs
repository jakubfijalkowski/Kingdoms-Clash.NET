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
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET.Server");
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
			if (this.Status == ClientStatus.Ok)
			{
				this.Send(new Message(MessageType.Close, null));
				this.Status = ClientStatus.Closed;
				this.Socket.Close();
				Logger.Info("Client {0}:{1} - connection closed", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
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

		#region Protected Members
		protected override bool HandleNewMessage(Message msg)
		{
			switch (msg.Type)
			{
				case MessageType.TooManyConnections: //Tą wiadomość możemy ignorować - tylko serwer może ją wysłać
					return false;

				case MessageType.Welcome: //Jeśli wysłano ją w innym momencie niż przy sekwencji powitalnej - ignorujemy
					return this.Status == ClientStatus.Welcome;

				case MessageType.AllOk: //Tylko serwer może ją wysłać
					return false;

				case MessageType.Close: //Zamknięcie połączenia
					this.CloseSocket();
					this.Status = ClientStatus.Closed;
					Logger.Info("Client {0}:{1} closed the connection", this.RemoteEndpoint.Address, this.RemoteEndpoint.Port);
					return false;
			}
			return true;
		}
		#endregion

		#region Internal fields
		/// <summary>
		/// Zamyka gniazdo.
		/// </summary>
		internal void CloseSocket()
		{
			this.Socket.Close();
		}
		
		/// <summary>
		/// Odbiera dane z socketu.
		/// </summary>
		internal void Receive()
		{
			base.Receive();
		}
		#endregion
	}
}
