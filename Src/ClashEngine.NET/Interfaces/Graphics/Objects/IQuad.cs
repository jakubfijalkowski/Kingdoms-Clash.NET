using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Prostokąt.
	/// </summary>
	public interface IQuad
		: IObject
	{
		/// <summary>
		/// Pozycja prostokąta.
		/// </summary>
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar prostokąta.
		/// </summary>
		Vector2 Size { get; set; }

		/// <summary>
		/// Kolor prostokąta.
		/// </summary>
		Vector4 Color { get; set; }
	}
}
