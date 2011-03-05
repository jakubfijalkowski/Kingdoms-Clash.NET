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
		/// <remarks>
		/// Brak dodatkowych danych.
		/// </remarks>
		TooManyConnections = 0x0001,

		/// <summary>
		/// Wiadomość powitalna.
		/// </summary>
		/// <remarks>
		/// Dodatkowe dane:
		///  4B - wersja serwera(major, minor, build, revision)
		///  1B - flagi:
		///  [
		///    - 0b - czy potrzebne jest hasło
		///  ]
		///  2B - długość nazwy gry,
		///  (długość nazwy)*2B - nazwa gry,
		/// 
		/// Klient musi odpowiedzieć na tą wiadomość w takim formacie:
		///  4B - wersja klienta(major, minor, build, revision)
		///  [
		///   2B - długość hasła,
		///   (długość hasła)*2B - hasło
		///  ]
		/// </remarks>
		Welcome = 0x0002,

		/// <summary>
		/// Komenda użytkownika - wartości większe od UserCommand.
		/// </summary>
		UserCommand = 0x0100,

		/// <summary>
		/// Wartość określająca koniec wiadomości.
		/// </summary>
		MessageEnd = 0xFFFF
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
		public Message(byte[] data)
		{
			System.Diagnostics.Debug.Assert(data.Length > 2, "Invalid message");
			if (BitConverter.IsLittleEndian)
			{
				this.Type = (MessageType)BitConverter.ToUInt16(data, 0);
			}
			else
			{
				byte[] tmp = new byte[] { data[1], data[0] };
				this.Type = (MessageType)BitConverter.ToUInt16(tmp, 0);
			}
			this.Data = new byte[data.Length - 4];
			System.Array.Copy(data, 2, this.Data, 0, data.Length - 4);
		}
		#endregion	
	}
}
