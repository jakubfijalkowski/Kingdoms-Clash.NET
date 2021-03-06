﻿using System;

namespace ClashEngine.NET.Interfaces.Net
{
	using Collections;

	/// <summary>
	/// Lista klientów.
	/// Użytkownik nie może ręcznie dodawać klientów za to może ich usuwać(co wymusza zamknięcie połączenia).
	/// </summary>
	public interface IClientsCollection
		: ISafeList<IClient>
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

		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów.
		/// Nie lockuje kolekcji.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		void SendToAllNoLock(Message msg);

		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów umożliwiając ich filtorwanie.
		/// Nie lockuje kolekcji.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		/// <param name="pred">Predykat.</param>
		void SendToAllNoLock(Message msg, Predicate<IClient> pred);
	}
}
