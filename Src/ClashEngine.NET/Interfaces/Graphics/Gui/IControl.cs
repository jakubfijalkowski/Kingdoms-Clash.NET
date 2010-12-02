namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Bazowy interfejs dla kontrolki GUI.
	/// </summary>
	public interface IControl
	{
		/// <summary>
		/// Identyfikator kontrolki.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Dane UI dla kontrolki.
		/// </summary>
		IUIData Data { get; set; }

		/// <summary>
		/// Pozycja kontrolki.
		/// </summary>
		OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar kontrolki.
		/// </summary>
		OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Czy kontrolka ma być "permanentnie" aktywna, tzn. czy po puszczeniu przycisku myszy przestaje być aktywna.
		/// </summary>
		bool PermanentActive { get; }

		/// <summary>
		/// Czy kontrolka jest aktywna.
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		/// Czy kontrolka jest "gorąca".
		/// </summary>
		bool IsHot { get; }

		/// <summary>
		/// Czy kontrolka jest widoczna.
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// Kolekcja z obiektami renderera dla kontrolki.
		/// </summary>
		IObjectsCollection Objects { get; }

		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns>Prawda, gdy myszka jest nad kontrolką. W przeciwnym razie fałsz.</returns>
		bool ContainsMouse();

		/// <summary>
		/// Uaktualnia kontrolkę.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Renderuje kontrolkę.
		/// </summary>
		void Render();

		/// <summary>
		/// Sprawdza, czy zaszła jakaś akcja kontrolki(np. czy przycisk został wciśnięty).
		/// </summary>
		/// <returns>Nr akcji lub 0, gdy takowa nie zaszła.</returns>
		int Check();
	}
}
