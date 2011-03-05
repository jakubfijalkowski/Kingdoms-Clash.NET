using System.Net;
using System;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Interfejs dla klas serwera.
	/// </summary>
	/// <remarks>
	/// Dane binarne przesyłane są zawsze w little endian.
	/// Ciągi znaków predefiniowanych wiadomości przesyłane są jako UTF-16LE poprzedzony dwoma bajtami określającymi ich długość.
	/// Każda wiadomość kończy się specjalnym ciągiem bajtów określającym jej koniec: <see cref="MessageType.MessageEnd"/>.
	/// Każda wiadomość wysyłana i odbierana przez serwer ma następujący format:
	///  * 2B - wiadomość z <see cref="MessageType"/>
	///  * dane zależne od wiadomości.
	///  * 2B - oznaczenie końca wiadomości.
	///  
	/// Sekwencja powitalna:
	///  * klient łączy się
	///  * serwer wysyła wiadomość Welcome
	///  * klient odpowiada wiadomością Welcome
	///  * jeśli wszystko się zgadza serwer wysyła wiadomość <see cref="MessageType.AllOk"/>, jeśli wersja jest niekompatybilna serwer wysyła wiadomość <see cref="MessageType.IncopatibleVersion"/>
	/// </remarks>
	public interface IServer
	{
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
		/// Czy serwer jest uruchomiony.
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// Nazwa gry.
		/// </summary>
		string GameName { get; }

		/// <summary>
		/// Wersja gry.
		/// </summary>
		Version GameVersion { get; }

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
