namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Typy wiadomości dla użytkownika.
	/// </summary>
	public enum GameMessageType
		: ushort
	{
		/// <summary>
		/// Pierwsza "konfiguracja" użytkownika.
		/// Wysyłana jako pierwsza wiadomość klienta po poprawnym połączeniu.
		/// </summary>
		PlayersFirstConfiguration = ClashEngine.NET.Interfaces.Net.MessageType.UserCommand + 1,

		/// <summary>
		/// Wysyłane przez serwer do klienta po zaakceptowaniu go.
		/// </summary>
		PlayerAccepted,

		/// <summary>
		/// Połączenie się użytkownika.
		/// </summary>
		PlayerConnected,

		/// <summary>
		/// Rozłączenie użytkownika.
		/// </summary>
		PlayerDisconnected,

		/// <summary>
		/// Użytkownik zmienił nick.
		/// </summary>
		PlayerChangedNick
	}
}
