namespace ClashEngine.NET.Interfaces.Rendering
{
	/// <summary>
	/// Tryb sortowania.
	/// </summary>
	public enum SortMode
	{
		/// <summary>
		/// Sortowanie obiektów po teksturze.
		/// </summary>
		Texture,

		/// <summary>
		/// Sortowanie obiektów po głębokości.
		/// Mniejsze wartości są wyświetlane pierwsze.
		/// </summary>
		FrontToBack,

		/// <summary>
		/// Sortowanie obiektów po głębokości.
		/// Większe wartości są wyświetlane pierwsze.
		/// </summary>
		BackToFront
	}

	/// <summary>
	/// Renderer.
	/// </summary>
	public interface IRenderer
	{
		/// <summary>
		/// Tryb sortowania.
		/// </summary>
		SortMode SortMode { get; set; }

		/// <summary>
		/// Rysuje obiekt.
		/// </summary>
		/// <param name="obj">Obiekt do odrysowania.</param>
		void Draw(IObject obj);

		/// <summary>
		/// Wyświetla wszystkie rysowane obiekty(<see cref="Draw"/>).
		/// </summary>
		void Render();
	}
}
