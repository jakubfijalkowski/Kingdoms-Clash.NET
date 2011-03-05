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
		Closed
	}

	/// <summary>
	/// Klient.
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Informacje o kliencie.
		/// </summary>
		IPEndPoint Endpoint { get; }

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
		void Open();

		/// <summary>
		/// Zamyka połączenie z klientem.
		/// </summary>
		void Close();

		/// <summary>
		/// Wysyła wskazaną wiadomość do klienta.
		/// </summary>
		/// <param name="message">Wiadomość.</param>
		void Send(Message message);

		/// <summary>
		/// Odbiera, jeśli są, dane i, jeśli może, parsuje je na obiekty typu <see cref="Message"/> dodając do kolekcji <see cref="Messages"/>.
		/// </summary>
		void Receive();

		/// <summary>
		/// Przygotowuje klienta do współpracy z serwerem(wymiana podstawowych wiadomości).
		/// </summary>
		void Prepare();
	}
}
