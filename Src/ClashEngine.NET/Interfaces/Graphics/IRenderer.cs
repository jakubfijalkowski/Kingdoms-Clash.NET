namespace ClashEngine.NET.Interfaces.Graphics
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
		/// Musi być wywołana pomiędzy <see cref="Begin"/> i <see cref="End"/>.
		/// </summary>
		/// <param name="obj">Obiekt do odrysowania.</param>
		void Draw(IObject obj);

		/// <summary>
		/// Rozpoczyna rysowanie.
		/// </summary>
		void Begin();

		/// <summary>
		/// Kończy rysowanie.
		/// </summary>
		void End();

		/// <summary>
		/// Opróżnia renderer.
		/// Musi być wywołana pomiędzy <see cref="Begin"/> i <see cref="End"/>.
		/// </summary>
		void Flush();
	}
}
