using System;
using System.Net;

namespace ClashEngine.NET.Interfaces.Net
{
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
		/// Czy serwer jest uruchomiony.
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// Startuje serwer na nowym wkątku.
		/// </summary>
		void Start();

		/// <summary>
		/// Zatrzymuje działanie serwera rozłączając wszystkich klientów i kończąc dodatkowy wątek.
		/// </summary>
		void Stop();
	}
}
