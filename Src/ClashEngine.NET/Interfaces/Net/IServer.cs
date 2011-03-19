using System;
using System.Net;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Stan serwera.
	/// </summary>
	public enum ServerState
	{
		/// <summary>
		/// Zatrzymany.
		/// </summary>
		Stopped,

		/// <summary>
		/// W trakcie włączania.
		/// </summary>
		Starting,

		/// <summary>
		/// Działa.
		/// </summary>
		Running,

		/// <summary>
		/// Wystąpił błąd.
		/// </summary>
		Error
	}

	/// <summary>
	/// Interfejs dla klas serwera.
	/// </summary>
	/// <remarks>
	/// Dane binarne przesyłane są zawsze w little endian.
	/// Ciągi znaków predefiniowanych wiadomości przesyłane są jako UTF-16LE poprzedzony dwoma bajtami określającymi ich długość.
	/// Każda wiadomość kończy się specjalnym ciągiem bajtów określającym jej koniec: 2x<see cref="MessageType.MessageEnd"/>.
	/// Każda wiadomość wysyłana i odbierana przez serwer ma następujący format:
	///  * 2B - wiadomość z <see cref="MessageType"/>
	///  * dane zależne od wiadomości.
	///  * 4B - oznaczenie końca wiadomości(2x<see cref="MessageType.MessageEnd"/>).
	///  
	/// Sekwencja powitalna:
	///  * klient łączy się
	///  * jeśli nie ma wolnych miejsc serwer wysyła wiadomość <see cref="MessageType.TooManyConnections"/>
	///  * serwer wysyła wiadomość <see cref="MessageType.Welcome"/>
	///  * jeśli klient nie może dostosować się do wersji wysyła wiadomość <see cref="MessageType.IncopatibleVersion"/> i kończy połączenie
	///  * klient odpowiada wiadomością <see cref="MessageType.Welcome"/>
	///  * jeśli wersja jest niekompatybilna serwer wysyła wiadomość <see cref="MessageType.IncopatibleVersion"/> i kończy połączenie
	///  * jeśli coś się nie zgadza z sekwencją powitalną serwer wysyła wiadomość <see cref="MessageType.InvalidSequence"/> i kończy połączenie
	///  * jeśli wszystko się zgadza serwer wysyła wiadomość <see cref="MessageType.AllOk"/>
	///  * jeśli klient nie potrafi rozpoznać sekwencji powitalnej wysyła <see cref="MessageType.InvalidSequence"/> i kończy połączenie
	///  
	/// Zakończenie połączenia:
	///  * jedna ze stron wysyła wiadomość <see cref="MessageType.Close"/> i kończy połączenie
	///  
	/// Kanał informacji:
	/// Aby otrzymać informacje o serwerze należy wysłać na kanał informacji PUSTĄ wiadomość <see cref="MessageType.Welcome"/>.
	/// Serwer odpowie wysyłając pakiet zawierający:
	///  * 2B - długość nazwy
	///  * (długość nazwy) * 2B - nazwa
	///  * 4B - wersja serwera
	///  * 4B - aktualna liczba podłączonych do niego klientów
	///  * 4B - maksymalna liczba klientów
	///  * 2B - długość danych dodatkowych
	///  * (długość danych dodatkowych) * 2B - dane dodatkowe
	/// </remarks>
	public interface IServer
	{
		/// <summary>
		/// Nazwa gry.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Wersja.
		/// </summary>
		Version Version { get; }

		/// <summary>
		/// Dane, na których serwer nasłuchuje.
		/// </summary>
		IPEndPoint Endpoint { get; }

		/// <summary>
		/// Dane, na których otwarty jest kanał informacyjny.
		/// Null, jeśli taki kanał nie jest otwarty.
		/// </summary>
		IPEndPoint InfoEndpoint { get; }

		/// <summary>
		/// Maksymalna liczba klientów podłączonych do serwera.
		/// </summary>
		uint MaxClients { get; }

		/// <summary>
		/// Lista klientów aktualnie podłączonych do serwera.
		/// </summary>
		IClientsCollection Clients { get; }

		/// <summary>
		/// Maksymlny czas braku aktywności ze strony klienta. Po jego przekroczeniu klient jest rozłączany.
		/// </summary>
		TimeSpan MaxClientIdleTime { get; }

		/// <summary>
		/// Stan serwera.
		/// </summary>
		ServerState State { get; }

		/// <summary>
		/// Startuje serwer na nowym wątku.
		/// </summary>
		/// <param name="wait">Określa, czy czekać do pełnego uruchomienia serwera.</param>
		void Start(bool wait = true);

		/// <summary>
		/// Zatrzymuje działanie serwera rozłączając wszystkich klientów i kończąc dodatkowy wątek.
		/// </summary>
		/// <param name="wait">Określa, czy czekać do pełnego zatrzymania serwera.</param>
		void Stop(bool wait = true);
	}
}
