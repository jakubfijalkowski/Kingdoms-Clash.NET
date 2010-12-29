namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Bazowy interfejs dla obiektu gry.
	/// </summary>
	public interface IGame
	{
		#region Properties
		/// <summary>
		/// Okno gry.
		/// </summary>
		IWindow Window { get; }

		/// <summary>
		/// Manager ekranów dla gry.
		/// </summary>
		IScreensManager Screens { get; }

		/// <summary>
		/// Manager zasobów.
		/// </summary>
		IResourcesManager Content { get; }

		/// <summary>
		/// Renderer.
		/// </summary>
		Graphics.IRenderer Renderer { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizacja gry.
		/// </summary>
		void OnInit();

		/// <summary>
		/// Deinicjalizacja gry.
		/// </summary>
		void OnDeinit();

		/// <summary>
		/// Metoda do uaktualnień.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void OnUpdate(double delta);

		/// <summary>
		/// Odrysowywanie.
		/// </summary>
		void OnRender();

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
