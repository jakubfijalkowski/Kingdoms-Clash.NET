using System;
using System.Net;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Status klienta.
	/// </summary>
	public enum ClientStatus
	{
		/// <summary>
		/// Wszystko ok.
		/// </summary>
		Ok,

		/// <summary>
		/// W trakcie sekwencji powitalnej.
		/// </summary>
		Welcome,

		/// <summary>
		/// Niekompatybilna wersja.
		/// </summary>
		IncompatibleVersion,

		/// <summary>
		/// Zbyt dużo połączeń z serwerem.
		/// </summary>
		TooManyConnections,

		/// <summary>
		/// Wysłaliśmy niepoprawną sekwencję.
		/// </summary>
		InvalidSequence,

		/// <summary>
		/// Inny błąd(najczęściej związany z gniazdem).
		/// </summary>
		Error,

		/// <summary>
		/// Połączenie zamknięte.
		/// </summary>
		Closed,

		/// <summary>
		/// Druga strona nie odpowiada.
		/// </summary>
		NotResponding
	}

	/// <summary>
	/// Klient.
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Adres docelowy.
		/// </summary>
		IPEndPoint RemoteEndpoint { get; }

		/// <summary>
		/// Adres lokalny.
		/// </summary>
		IPEndPoint LocalEndpoint { get; }

		/// <summary>
		/// Kolejka wiadomości.
		/// </summary>
		IMessagesCollection Messages { get; }

		/// <summary>
		/// Status połączenia.
		/// </summary>
		ClientStatus Status { get; }

		/// <summary>
		/// Wersja klienta.
		/// </summary>
		Version Version { get; }

		/// <summary>
		/// Data ostatniej aktywności drugiej strony.
		/// </summary>
		DateTime LastAction { get; }

		/// <summary>
		/// Otwiera połączenie.
		/// </summary>
		/// <param name="wait">Określa, czy czekać na otwarcie połączenia i zakończenie sekwencji powitalnej.</param>
		void Open(bool wait = true);

		/// <summary>
		/// Zamyka połączenie.
		/// </summary>
		/// <param name="wait">Określa, czy czekać na zakończenie połączenia.</param>
		void Close(bool wait = true);

		/// <summary>
		/// Wysyła wskazaną wiadomość.
		/// </summary>
		/// <param name="message">Wiadomość.</param>
		void Send(Message message);
	}
}
