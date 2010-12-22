namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Bazowy interfejs dla obiektu gry.
	/// </summary>
	/// <remarks>
	/// Odpowiedzialny jest za wejście, wyświetlanie i fizykę.
	/// </remarks>
	public interface IGame
	{
		#region Properties
		/// <summary>
		/// Nazwa gry.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Rozmiar okna gry.
		/// </summary>
		OpenTK.Vector2 Size { get; }

		/// <summary>
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		bool VSync { get; }

		/// <summary>
		/// Czy okno jest pełnoekranowe.
		/// </summary>
		bool IsFullscreen { get; }

		/// <summary>
		/// Czy okno jest aktywne.
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		/// Tryb graficzny.
		/// </summary>
		OpenTK.Graphics.GraphicsMode Mode { get; }

		/// <summary>
		/// Manager ekranów dla gry.
		/// </summary>
		IScreensManager Screens { get; }

		/// <summary>
		/// Manager zasobów.
		/// </summary>
		IResourcesManager Content { get; }

		/// <summary>
		/// Wejście.
		/// </summary>
		IInput Input { get; }

		/// <summary>
		/// Renderer.
		/// </summary>
		Graphics.IRenderer Renderer { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizacja gry.
		/// Wywoływana np. przy ładowaniu okna.
		/// </summary>
		void Init();

		/// <summary>
		/// Deinicjalizacja gry.
		/// Wywoływana np. przy zamykaniu okna.
		/// </summary>
		void DeInit();

		/// <summary>
		/// Metoda do uaktualnień.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void Update(double delta);

		/// <summary>
		/// Odrysowywanie.
		/// </summary>
		void Render();

		/// <summary>
		/// Uruchamia grę z maksymalną wydajnością.
		/// </summary>
		void Run();

		/// <summary>
		/// Zamyka grę.
		/// </summary>
		void Exit();
		#endregion
	}
}
