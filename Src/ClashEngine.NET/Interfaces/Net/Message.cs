using System;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Wiadomości wysyłane przez serwer.
	/// </summary>
	public enum MessageType
		: ushort
	{
		/// <summary>
		/// Zbyt dużo połączeń.
		/// </summary>
		TooManyConnections = 0x0001,

		/// <summary>
		/// Wiadomość powitalna.
		/// </summary>
		/// <remarks>
		/// Dodatkowe dane:
		///  * 4B - wersja serwera(major, minor, build, revision)
		///  * 1B - flagi:
		///  * 2B - długość nazwy gry,
		///  * (długość nazwy)*2B - nazwa gry,
		/// 
		/// Klient musi odpowiedzieć na tą wiadomość w takim formacie:
		///  * 4B - wersja klienta(major, minor, build, revision)
		/// </remarks>
		Welcome = 0x0002,

		/// <summary>
		/// Wszystko w porządku, można przekazać działanie użytkownikowi.
		/// </summary>
		AllOk,

		/// <summary>
		/// Niekompatybilna wersja.
		/// </summary>
		IncompatibleVersion,

		/// <summary>
		/// Niepoprawna sekwencja powitalna.
		/// </summary>
		InvalidSequence,

		/// <summary>
		/// Zamknięcie połączenia.
		/// </summary>
		Close,

		/// <summary>
		/// Klient zbyt długo nie odpowiada.
		/// </summary>
		TimeOut,

		/// <summary>
		/// Komenda użytkownika - wartości większe od UserCommand.
		/// </summary>
		UserCommand = 0x0100,

		/// <summary>
		/// Wartość określająca koniec wiadomości.
		/// </summary>
		MessageEnd = 0xDEAD,
	}

	/// <summary>
	/// Pojedyncza wiadomość.
	/// </summary>
	public struct Message
	{
		/// <summary>
		/// Typ wiadomości.
		/// </summary>
		public MessageType Type;

		/// <summary>
		/// Dane wiadomości.
		/// </summary>
		public byte[] Data;

		#region Constructors
		/// <summary>
		/// Inicjalizuje nową wiadomość typem i danymi.
		/// </summary>
		/// <param name="type">Typ.</param>
		/// <param name="data">Dane.</param>
		public Message(MessageType type, byte[] data)
		{
			this.Type = type;
			this.Data = data;
		}

		/// <summary>
		/// Inicjalizuje(parsuje) otrzymaną wiadomość.
		/// </summary>
		/// <param name="data">Dane w formacie przedstawionym w opisie <see cref="IServer"/>.</param>
		public Message(byte[] data, int start, int length)
		{
			if (length < 6)
			{
				throw new ArgumentException("Insufficient data");
			}
			if (BitConverter.IsLittleEndian)
			{
				this.Type = (MessageType)BitConverter.ToUInt16(data, start);
			}
			else
			{
				byte[] tmp = new byte[] { data[start + 1], data[start] };
				this.Type = (MessageType)BitConverter.ToUInt16(tmp, 0);
			}
			this.Data = new byte[length - 6];
			System.Array.Copy(data, start + 2, this.Data, 0, length - 6);
		}
		#endregion	
	}
}
