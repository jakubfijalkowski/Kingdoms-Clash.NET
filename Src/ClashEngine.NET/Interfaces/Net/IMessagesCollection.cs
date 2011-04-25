namespace ClashEngine.NET.Interfaces.Net
{
	using Collections;

	/// <summary>
	/// Kolejka wiadomości.
	/// Nie wspiera dodawania nowych wiadomości z zewnątrz.
	/// </summary>
	public interface IMessagesCollection
		: ISafeList<Message>
	{
		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wiadomość o wskazanym typie.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		bool Contains(MessageType type);

		/// <summary>
		/// Pobiera indeks wiadomości o wskazanym typie lub zwraca -1.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		int IndexOf(MessageType type);

		/// <summary>
		/// Pobiera pierwszą wiadomość o wskazanym typie.
		/// </summary>
		/// <param name="type">Typ wiadomości.</param>
		/// <returns></returns>
		Message GetFirst(MessageType type);
	}
}
