using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Components
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Wierzchołek terenu.
	/// </summary>
	public class TerrainVertex
	{
		/// <summary>
		/// Pozycja wierzchołka.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Kolor wierzchołka.
		/// TODO: zamienić na teksturę.
		/// </summary>
		public Vector4 Color;
	}

	/// <summary>
	/// Komponent-teren 2D.
	/// Buduje teren za pomocą wierzchołków wierzchniej warstwy(to co jest "pod" jest dedukowane automatycznie) i wysokości terenu.
	/// </summary>
	/// <remarks>
	/// W przyszłości będzie zintegrowany z fizyką.
	/// </remarks>
	public interface ITerrain
		: IRenderableComponent
	{
		/// <summary>
		/// Wierzchołki terenu.
		/// Tylko do odczytu.
		/// </summary>
		IList<TerrainVertex> Vertices { get; }

		/// <summary>
		/// Wysokość mapy.
		/// </summary>
		float Height { get; }
	}
}
