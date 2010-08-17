namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Bazowy interfejs dla obiektu gry.
	/// </summary>
	public interface IGame
	{
		#region Properties
		/// <summary>
		/// Nazwa gry.
		/// </summary>
		string Name { get; }

		#region Managers
		/// <summary>
		/// Manager ekranów dla gry.
		/// </summary>
		ScreensManager.IScreensManager ScreensManager { get; }

		/// <summary>
		/// Manager zasobów.
		/// </summary>
		ResourcesManager.IResourcesManager ResourcesManager { get; }
		#endregion

		#region Window info
		/// <summary>
		/// Szerokość okna.
		/// </summary>
		int Width { get; }

		/// <summary>
		/// Wysokość okna.
		/// </summary>
		int Height { get; }

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
		#endregion
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
		/// Uruchamia grę ze stałą liczbą wywołań metod Update i Render.
		/// </summary>
		/// <param name="updatesPerSecond">Liczba uaktualnień i odrysowań na sekundę.</param>
		void Run(double updatesPerSecond);

		/// <summary>
		/// Uruchamia grę ze stałą liczbą wywołań metod Update i Render.
		/// </summary>
		/// <param name="updatesPerSecond">Liczba uaktualnień na sekundę.</param>
		/// <param name="framesPerSecond">Liczba klatek na sekundę.</param>
		void Run(double updatesPerSecond, double framesPerSecond);
		#endregion
	}
}
