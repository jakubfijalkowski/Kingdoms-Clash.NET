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
		PlayerChangedNick,

		/// <summary>
		/// Zmienia stan gracza - obserwator/ready-to-play.
		/// </summary>
		PlayerChangedState,

		/// <summary>
		/// Wiadomość określająca za ile rozpocznie się mecz.
		/// </summary>
		GameWillStartAfter,

		/// <summary>
		/// Gra rozpoczęta.
		/// </summary>
		GameStarted,

		/// <summary>
		/// Gra zakończona.
		/// </summary>
		GameEnded,

		/// <summary>
		/// Stworzenia/usunięcie jednostki z kolejki produkcyjnej.
		/// </summary>
		UnitQueueAction,

		/// <summary>
		/// Odpowiedź serwera na tworzenie jednostki - czy udało się ją stworzyć.
		/// </summary>
		UnitQueued,

		/// <summary>
		/// Jednostka została stworzona.
		/// </summary>
		UnitCreated,

		/// <summary>
		/// Jednostka została zniszczona.
		/// </summary>
		UnitDestroyed,

		/// <summary>
		/// Gracz został zraniony - zmniejszyło się HP jego zamku.
		/// </summary>
		PlayerHurt
	}

	/// <summary>
	/// Powód rozłączenia gracza.
	/// </summary>
	public enum DisconnectionReason
		: byte
	{
		/// <summary>
		/// Błąd połączenia.
		/// </summary>
		ConnectionError = ClashEngine.NET.Interfaces.Net.ClientStatus.Error,
		
		/// <summary>
		/// Przekroczono czas na odpowiedź.
		/// </summary>
		TimedOut = ClashEngine.NET.Interfaces.Net.ClientStatus.NotResponding,

		/// <summary>
		/// Akcja użytkownika.
		/// </summary>
		UserAction = ClashEngine.NET.Interfaces.Net.ClientStatus.Closed
	}
}
