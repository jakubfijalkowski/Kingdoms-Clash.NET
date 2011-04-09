using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Lista klientów.
	/// Użytkownik nie może ręcznie dodawać klientów za to może ich usuwać(co wymusza zamknięcie połączenia).
	/// </summary>
	public interface IClientsCollection
		: IList<IClient>
	{
		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		void SendToAll(Message msg);

		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów umożliwiając ich filtorwanie.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		/// <param name="pred">Predykat.</param>
		void SendToAll(Message msg, Predicate<IClient> pred);
	}
}
