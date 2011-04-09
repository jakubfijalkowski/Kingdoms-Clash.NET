using System;

namespace ClashEngine.NET.Net.Messages
{
	using Interfaces.Net;

	/// <summary>
	/// Wiadomość powitalna klienta.
	/// Klasa pomocnicza.
	/// </summary>
	public struct ClientWelcome
	{
		/// <summary>
		/// Wersja serwera.
		/// </summary>
		public Version Version;

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="serverVersion">Wersja.</param>
		public ClientWelcome(Version version)
		{
			this.Version = version;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public ClientWelcome(Message msg)
		{
			if (msg.Type != MessageType.Welcome)
			{
				throw new InvalidCastException("Cannot convert this message to WelcomeMessage");
			}
			this.Version = new Version(msg.Data[0], msg.Data[1], msg.Data[2], msg.Data[3]);
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[4];
			BinarySerializer.StaticSerialize(data, (byte)this.Version.Major, (byte)this.Version.Minor, (byte)this.Version.Build, (byte)this.Version.Revision);
			return new Message(MessageType.Welcome, data);
		}
		#endregion
	}
}
