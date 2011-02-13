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
		float Depth { get; set; }

		/// <summary>
		/// Rotacja obiektu.
		/// </summary>
		float Rotation { get; set; }

		/// <summary>
		/// Wierzchołki obiektu.
		/// </summary>
		Vertex[] Vertices { get; }

		/// <summary>
		/// Indeksy wierzchołków.
		/// Jeśli jest null, renderer nie używa ich.
		/// </summary>
		int[] Indecies { get; }

		/// <summary>
		/// Wywoływane tuż przed wyrenderowaniem obiektu.
		/// </summary>
		//void PreRender();
	}
}
