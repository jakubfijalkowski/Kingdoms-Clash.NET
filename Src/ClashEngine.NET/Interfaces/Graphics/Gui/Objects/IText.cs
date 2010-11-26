namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	using Graphics.Resources;

	/// <summary>
	/// Tekst.
	/// </summary>
	public interface IText
		: IObject
	{
		/// <summary>
		/// Użyta czcionka.
		/// </summary>
		IFont Font { get; set; }

		/// <summary>
		/// Tekst.
		/// </summary>
		string TextValue { get; set; }

		/// <summary>
		/// Kolor.
		/// </summary>
		OpenTK.Vector4 Color { get; set; }

		/// <summary>
		/// Pozycja.
		/// </summary>
		OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		OpenTK.Vector2 Size { get; set; }
	}
}
