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
		#region Statics
		private const int BufferSize = 2048;
		private static readonly byte[] EndMessage = null;
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		#endregion

		#region Private fields
		private Socket Socket = null;
		private byte[] Buffer = new byte[BufferSize];
		private int BufferIndex = 0;
		private Internals.MessagesCollection _Messages = new MessagesCollection();
		#endregion

		#region Internal fields
		internal Messages.ClientWelcome? WelcomeMessage = null;
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
		/// Kolejka wiadomości.
		/// </summary>
		public IMessagesCollection Messages
		{
			get { return this._Messages; }
		}

		/// <summary>
		/// Status z serwerem.
		/// Możliwe wartości:
		///		* <see cref="MessageType.AllOk"/> - wszystko ok, można przejść dalej
		///		* <see cref="MessageType.IncompatibleVersion"/> - niekompatybilna wersja, połączenie zakończone
		///		* <see cref="MessageType.Welcome"/> - jeszcze nie skończono sekwencji powitalnej
		/// </summary>
		public MessageType Status { get; internal set; }

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
			if (!BitConverter.IsLittleEndian)
			{
				byte tmp = messageType[0];
				messageType[0] = messageType[1];
				messageType[1] = tmp;

				tmp = endMessage[0];
				endMessage[0] = endMessage[1];
				endMessage[1] = tmp;
			}
			this.Socket.Send(messageType);
			if (message.Data != null)
				this.Socket.Send(message.Data);
			this.Socket.Send(endMessage);
		}

		/// <summary>
		/// Odbiera, jeśli są, dane od serwera i, jeśli może, parsuje je na obiekty typu <see cref="Message"/>.
		/// </summary>
		public void Receive()
		{
			if (this.Socket.Poll(0, SelectMode.SelectRead))
			{
				int start = 0;
				int i = this.BufferIndex;
				this.BufferIndex += this.Socket.Receive(this.Buffer, this.BufferIndex, BufferSize - this.BufferIndex, SocketFlags.None);
				int messageEnd = -1;
				do
				{
					messageEnd = -1;
					for (; i < this.BufferIndex - 1; i++)
					{
						if (this.Buffer[i] == EndMessage[0] && this.Buffer[i + 1] == EndMessage[1]) //Mamy koniec wiadomości
						{
							messageEnd = (i += 2);
							try
							{
								this._Messages.InternalAdd(new Message(this.Buffer, start, messageEnd - start));
							}
							catch (Exception ex)
							{
								Logger.WarnException(string.Format("Cannot parse message from {0}", this.Endpoint.Address), ex);
							}
							start = messageEnd;
							continue;
						}
					}
				} while (messageEnd != -1);
				if (start != 0)
				{
					Array.Copy(this.Buffer, start, this.Buffer, 0, this.BufferIndex - start);
					this.BufferIndex -= start;
				}
			}
		}

		/// <summary>
		/// Przygotowuje klienta do współpracy z serwerem(wymiana podstawowych wiadomości).
		/// </summary>
		public void Prepare()
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
		{
			this.Socket = socket;
			this.Socket.Blocking = false;
			this.Status = MessageType.Welcome;
		}

		static ServerClient()
		{
			if (BitConverter.IsLittleEndian)
			{
				EndMessage = new byte[]
					{
						((ushort)MessageType.MessageEnd) & 0x00FF,
						(((ushort)MessageType.MessageEnd) & 0xFF00) >> 8
					};
			}
			else
			{
				//TODO: to jest poprawna konwersja na big-endian?
				EndMessage = new byte[]
					{
						(((ushort)MessageType.MessageEnd) & 0xFF00) >> 8,
						((ushort)MessageType.MessageEnd) & 0x00FF
					};
			}
		}
		#endregion
	}
}
