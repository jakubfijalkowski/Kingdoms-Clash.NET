namespace ClashEngine.NET.Interfaces.Graphics.Components
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Komponent-teren 2D.
	/// Buduje teren za pomocą wierzchołków wierzchniej warstwy(to co jest "pod" jest dedukowane automatycznie) i wysokości terenu.
	/// </summary>
	public interface ITerrain
		: IRenderableComponent
	{
		/// <summary>
		/// Wysokość mapy.
		/// </summary>
		float Height { get; }
	}
}
