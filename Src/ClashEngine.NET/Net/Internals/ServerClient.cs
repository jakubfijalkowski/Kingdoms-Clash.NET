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
		: IClient
	{
		#region Private fields
		private Socket Socket = null;
		#endregion

		#region IClient Members
		/// <summary>
		/// Dane klienta.
		/// </summary>
		public IPEndPoint Endpoint
		{
			get { return (IPEndPoint)this.Socket.RemoteEndPoint; }
		}

		/// <summary>
		/// Zamyka połączenie z klientem.
		/// </summary>
		public void Close()
		{
		}

		/// <summary>
		/// Wysyła wskazaną wiadomość do klienta.
		/// </summary>
		/// <param name="message">Wiadomość.</param>
		public void Send(Message message)
		{
			byte[] messageType = BitConverter.GetBytes((ushort)message.Type);
			byte[] endMessage = BitConverter.GetBytes((ushort)MessageType.MessageEnd);
			if(!BitConverter.IsLittleEndian)
			{
				byte tmp = messageType[0];
				messageType[0] = messageType[1];
				messageType[1] = tmp;

				tmp = endMessage[0];
				endMessage[0] = endMessage[1];
				endMessage[1] = tmp;
			}
			this.Socket.Send(messageType);
			this.Socket.Send(message.Data);
			this.Socket.Send(endMessage);
		}
		#endregion

		#region Constructors
		public ServerClient(Socket socket)
		{
			this.Socket = socket;
		}
		#endregion
	}
}
