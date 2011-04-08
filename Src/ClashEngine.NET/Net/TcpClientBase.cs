using System;
using System.Net;
using System.Net.Sockets;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Bazowa klasa dla klienta TCP - obsługuje odbieranie i wysyłanie danych.
	/// </summary>
	public abstract class TcpClientBase
		: IClient
	{
		#region Statics
		private const int BufferSize = 2048;
		private static readonly byte[] EndMessage = null;
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		#endregion

		#region Private fields
		private byte[] Buffer = new byte[BufferSize];
		private int BufferIndex = 0;
		private Internals.MessagesCollection _Messages = new Internals.MessagesCollection();
		#endregion

		#region Protected fields
		protected Socket Socket = null;
		#endregion

		#region IClient Members
		/// <summary>
		/// Adres docelowy.
		/// </summary>
		public IPEndPoint RemoteEndpoint { get; private set; }

		/// <summary>
		/// Adres lokalny.
		/// </summary>
		public IPEndPoint LocalEndpoint
		{
			get
			{
				return (this.Socket != null ? (IPEndPoint)this.Socket.LocalEndPoint : null);
			}
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
		/// </summary>
		public ClientStatus Status { get; protected set; }

		/// <summary>
		/// Wersja klienta.
		/// </summary>
		public Version Version { get; protected set; }

		/// <summary>
		/// Data ostatniej aktywności drugiej strony.
		/// </summary>
		public DateTime LastAction { get; private set; }

		/// <summary>
		/// Dane użytkownika.
		/// </summary>
		public object UserData { get; set; }

		/// <summary>
		/// Otwiera połączenie.
		/// </summary>
		/// <param name="wait">Określa, czy czekać na otwarcie połączenia i zakończenie sekwencji powitalnej.</param>
		public abstract void Open(bool wait = true);

		/// <summary>
		/// Zamyka połączenie.
		/// </summary>
		/// <param name="wait">Określa, czy czekać na zakończenie połączenia.</param>
		public abstract void Close(bool wait = true);

		/// <summary>
		/// Wysyła wskazaną wiadomość do klienta.
		/// </summary>
		/// <param name="message">Wiadomość.</param>
		public virtual void Send(Message message)
		{
			if (this.Socket.Connected)
			{
				byte[] messageType = new byte[2];
				Utilities.NetBinarySerializer.Serialize(messageType, (ushort)message.Type);
				this.Socket.Send(messageType);
				if (message.Data != null)
					this.Socket.Send(message.Data);
				this.Socket.Send(EndMessage);
			}
		}
		#endregion

		#region Constructors
		public TcpClientBase(IPEndPoint endpoint)
		{
			this.RemoteEndpoint = endpoint;
		}

		static TcpClientBase()
		{
			EndMessage = new byte[4];
			Utilities.NetBinarySerializer.Serialize(EndMessage, ((ushort)MessageType.MessageEnd << 16) | (ushort)MessageType.MessageEnd);
		}
		#endregion

		#region Protected methods
		/// <summary>
		/// Obsługa nowych wiadomości przed dodaniem ich do kolekcji.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		/// <returns>Jeśli false - nie dodaje wiadomości do kolekcji.</returns>
		protected virtual bool HandleNewMessage(Message msg)
		{
			return true;
		}

		/// <summary>
		/// Odbiera, jeśli są, dane i, jeśli może, parsuje je na obiekty typu <see cref="Message"/> dodając do kolekcji <see cref="Messages"/>.
		/// </summary>
		protected void Receive(bool block = false)
		{
			if (this.Socket.Connected && (block || this.Socket.Poll(0, SelectMode.SelectRead)))
			{
				int start = 0;
				int i = this.BufferIndex;
				this.BufferIndex += this.Socket.Receive(this.Buffer, this.BufferIndex, BufferSize - this.BufferIndex, SocketFlags.None);
				this.LastAction = DateTime.Now;
				int messageEnd = -1;
				do
				{
					messageEnd = -1;
					for (; i < this.BufferIndex - 3; i++)
					{
						if (this.Buffer[i + 0] == EndMessage[0] &&
							this.Buffer[i + 1] == EndMessage[1] &&
							this.Buffer[i + 2] == EndMessage[2] &&
							this.Buffer[i + 3] == EndMessage[3]) //Mamy koniec wiadomości
						{
							messageEnd = (i += 4);
							try
							{
								var msg = new Message(this.Buffer, start, messageEnd - start);
								if (this.HandleNewMessage(msg))
								{
									this._Messages.InternalAdd(msg);
								}
							}
							catch (Exception ex)
							{
								Logger.WarnException(string.Format("Cannot parse message from {0}", this.RemoteEndpoint.Address), ex);
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
		#endregion
	}
}
