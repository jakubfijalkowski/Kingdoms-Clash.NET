using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics
{
	/// <summary>
	/// Wierzchołek.
	/// </summary>
	public struct Vertex
	{
		internal const int Size = (2 + 2 + 4) * sizeof(float);

		/// <summary>
		/// Pozycja.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Koordynaty tekstury.
		/// </summary>
		public Vector2 TexCoord;

		/// <summary>
		/// Kolor w formacie RGBA.
		/// </summary>
		public Vector4 Color;
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
		Resources.ITexture Texture { get; }

		/// <summary>
		/// Głębokość, na której znajduje się obiekt. Używane przy sortowaniu.
		/// </summary>
		float Depth { get; }

		/// <summary>
		/// Wierzchołki obiektu.
		/// </summary>
		Vertex[] Vertices { get; }

		///// <summary>
		///// Indeksy wierzchołków.
		///// </summary>
		//int[] Indecies { get; }
	}
}
