using System.Net;
using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Net
{
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
		/// Możliwe wartości:
		///		* <see cref="MessageType.AllOk"/> - wszystko ok, można przejść dalej
		///		* <see cref="MessageType.IncompatibleVersion"/> - niekompatybilna wersja, połączenie zakończone
		///		* <see cref="MessageType.Welcome"/> - jeszcze nie skończono sekwencji powitalnej
		///		* <see cref="MessageType.TooManyConnection"/> - nie udało się podłączyć - za dużo połączeń do serwera
		/// </summary>
		MessageType Status { get; }

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
		/// Odbiera, jeśli są, dane i, jeśli może, parsuje je na obiekty typu <see cref="Message"/>.
		/// </summary>
		void Receive();

		/// <summary>
		/// Przygotowuje klienta do współpracy z serwerem(wymiana podstawowych wiadomości).
		/// </summary>
		void Prepare();
	}
}
