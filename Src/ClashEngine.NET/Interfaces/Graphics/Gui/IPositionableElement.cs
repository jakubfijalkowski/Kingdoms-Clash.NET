namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Określa element który można pozycjonować.
	/// </summary>
	public interface IPositionableElement
	{
		/// <summary>
		/// Pozycja relatywna(nie uwzględnia ewentualnych offsetów).
		/// </summary>
		OpenTK.Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		OpenTK.Vector2 Size { get; set; }
	}
}
