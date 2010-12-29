namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Informacje o grze.
	/// </summary>
	public interface IGameInfo
	{
		/// <summary>
		/// Okno gry.
		/// </summary>
		IWindow MainWindow { get; }

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
	}
}
