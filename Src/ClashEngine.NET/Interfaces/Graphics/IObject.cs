using OpenTK;

namespace ClashEngine.NET.Interfaces.Rendering
{
	/// <summary>
	/// Wierzchołek.
	/// </summary>
	public struct Vertex
	{
		internal const int Size = (2 + 2) * sizeof(float);

		/// <summary>
		/// Pozycja.
		/// </summary>
		Vector2 Position;

		/// <summary>
		/// Koordynaty tekstury.
		/// </summary>
		Vector2 TexCoord;
	}

	/// <summary>
	/// Obiekt dla renderera.
	/// Obiekt jest wyświetlany jako trójkąty.
	/// </summary>
	public interface IObject
	{
		/// <summary>
		/// Tekstura obiektu.
		/// </summary>
		ClashEngine.NET.Interfaces.Resources.ITexture Texture { get; }

		/// <summary>
		/// Głębokość, na której znajduje się obiekt. Używane przy sortowaniu.
		/// </summary>
		float Depth { get; }

		/// <summary>
		/// Wierzchołki obiektu.
		/// </summary>
		Vertex[] Vertices { get; }

		/// <summary>
		/// Indeksy wierzchołków.
		/// </summary>
		int[] Indecies { get; }
	}
}
