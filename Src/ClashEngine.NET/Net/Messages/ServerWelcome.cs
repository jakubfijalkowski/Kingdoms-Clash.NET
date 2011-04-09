using System;

namespace ClashEngine.NET.Net.Messages
{
	using Interfaces.Net;

	/// <summary>
	/// Wiadomość powitalna.
	/// Klasa pomocnicza.
	/// </summary>
	public struct ServerWelcome
	{
		/// <summary>
		/// Wersja serwera.
		/// </summary>
		public Version ServerVersion;

		/// <summary>
		/// Nazwa gry.
		/// </summary>
		public string GameName;

		///// <summary>
		///// Czy jest potrzebne hasło.
		///// </summary>
		//public bool IsPasswordNeeded;

		#region Constructors
		/// <summary>
		/// Tworzy nową wiadomość.
		/// </summary>
		/// <param name="serverVersion">Wersja serwera.</param>
		/// <param name="gameName">Nazwa gry.</param>
		public ServerWelcome(Version serverVersion, string gameName)
		{
			this.ServerVersion = serverVersion;
			this.GameName = gameName;
		}

		/// <summary>
		/// Parsuje wiadomość.
		/// </summary>
		/// <param name="msg">Wiadomość.</param>
		public ServerWelcome(Message msg)
		{
			if (msg.Type != MessageType.Welcome)
			{
				throw new InvalidCastException("Cannot convert this message to WelcomeMessage");
			}
			this.ServerVersion = new Version(msg.Data[0], msg.Data[1], msg.Data[2], msg.Data[3]);

			byte flags = msg.Data[4];

			ushort strLength;
			if (BitConverter.IsLittleEndian)
			{
				strLength = BitConverter.ToUInt16(msg.Data, 5);
			}
			else
			{
				byte[] tmp = new byte[] { msg.Data[5], msg.Data[6] };
				strLength = BitConverter.ToUInt16(tmp, 0);
			}
			this.GameName = System.Text.Encoding.Unicode.GetString(msg.Data, 7, msg.Data.Length - 7);
			if (this.GameName.Length != strLength)
			{
				throw new InvalidCastException("Source message is invalid");
			}
		}
		#endregion

		#region ToMessage
		/// <summary>
		/// Zwraca wyspecjalizowaną wiadomość jako <see cref="Message"/>.
		/// </summary>
		/// <returns></returns>
		public Message ToMessage()
		{
			byte[] data = new byte[4 + 1 + 2 + this.GameName.Length * 2];
			BinarySerializer.StaticSerialize(data, (byte)this.ServerVersion.Major, (byte)this.ServerVersion.Minor,
				(byte)this.ServerVersion.Build, (byte)this.ServerVersion.Revision, (byte)0, this.GameName);
			return new Message(MessageType.Welcome, data);
		}
		#endregion
	}
}
