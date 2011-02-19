using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Punkt rotacji.
	/// </summary>
	public enum RotationPointSettings
	{
		/// <summary>
		/// Lewy górny róg.
		/// </summary>
		TopLeft,

		/// <summary>
		/// Lewy dolny róg.
		/// </summary>
		BottomLeft,

		/// <summary>
		/// Prawy górny róg.
		/// </summary>
		TopRight,

		/// <summary>
		/// Prawy dolny róg.
		/// </summary>
		BottomRight,

		/// <summary>
		/// Środek.
		/// </summary>
		Center,

		/// <summary>
		/// Ustawiany ręcznie.
		/// </summary>
		Custom
	}

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

		/// <summary>
		/// Punkt rotacji.
		/// </summary>
		new Vector2 RotationPoint { get; set; }

		/// <summary>
		/// Ustawienia automatycznego ustawiania punktu rotacji.
		/// </summary>
		RotationPointSettings RotationPointSettings { get; set; }
	}
}
