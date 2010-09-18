namespace ClashEngine.NET.Interfaces.Utilities
{
	/// <summary>
	/// Interfejs dla podwójnego VBO.
	/// <see cref="ClashEngine.NET.Utilities.VBO"/>
	/// </summary>
	public interface IVBO
	{
		/// <summary>
		/// Identyfikator VBO wierzchołków.
		/// </summary>
		int VerticesId { get; }

		/// <summary>
		/// Identyfikator VBO indeksów.
		/// </summary>
		int IndeciesId { get; }

		/// <summary>
		/// Liczba wierzchołków.
		/// </summary>
		int VerticesCount { get;}

		/// <summary>
		/// Liczba indeksów.
		/// </summary>
		int IndeciesCount { get;}

		/// <summary>
		/// Binduje VBO.
		/// </summary>
		void Bind();

		/// <summary>
		/// Rysuje VBO.
		/// </summary>
		void Draw();
	}
}
