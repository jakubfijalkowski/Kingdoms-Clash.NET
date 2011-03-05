using System;
using System.Net;
using System.Net.Sockets;

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
		/// Status z serwerem.
		/// Możliwe wartości:
		///		* <see cref="MessageType.AllOk"/> - wszystko ok, można przejść dalej
		///		* <see cref="MessageType.IncompatibleVersion"/> - niekompatybilna wersja, połączenie zakończone
		///		* <see cref="MessageType.Welcome"/> - jeszcze nie skończono sekwencji powitalnej
		/// </summary>
		public new MessageType Status
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
		{ }

		/// <summary>
		/// Przygotowuje klienta do współpracy z serwerem(wymiana podstawowych wiadomości).
		/// </summary>
		public override void Prepare()
		{
			this.Receive();
			int i = this.Messages.IndexOf(MessageType.Welcome);
			if (i >= 0)
			{
				try
				{
					this.WelcomeMessage = new Messages.ClientWelcome(this.Messages[i]);
				}
				catch (Exception ex)
				{
					Logger.WarnException("Client sent invalid Welcome message", ex);
				}
				this.Messages.RemoveAt(i);
			}
		}
		#endregion

		#region Constructors
		public ServerClient(Socket socket)
			: base((IPEndPoint)socket.RemoteEndPoint)
		{
			base.Socket = socket;
			base.Socket.Blocking = false;
			this.Status = MessageType.Welcome;
		}
		#endregion
	}
}
