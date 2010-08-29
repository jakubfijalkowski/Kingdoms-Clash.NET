namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Callback.
	/// </summary>
	/// <returns>Jeśli true callback zostanie usunięty z listy, w przeciwnym razie false.</returns>
	public delegate bool MainThreadCallback();

	/// <summary>
	/// Interfejs dla managera globalnych callbacków.
	/// Callbacki muszą być ZAWSZE uruchamiane w głównym wątku aplikacji, w tym samym gdzie stworzone zostało okno i kontekst OpenGL
	/// Instancja powinna być jedna na całą aplikacje(np. singleton).
	/// </summary>
	public interface IMainThreadCallbacksManager
	{
		/// <summary>
		/// Dodaje callback do listy.
		/// </summary>
		/// <param name="callback">Callback.</param>
		void Add(MainThreadCallback callback);

		/// <summary>
		/// Wywołuje WSZYSTKIE dodane callbacki i usuwa te, które tego zarządają.
		/// </summary>
		void Call();
	}
}
