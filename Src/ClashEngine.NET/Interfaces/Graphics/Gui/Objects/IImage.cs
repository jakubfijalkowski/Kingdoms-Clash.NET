namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	/// <summary>
	/// Obrazek.
	/// </summary>
	public interface IImage
		: IObject
	{
		/// <summary>
		/// Ścieżka do obrazka.
		/// </summary>
		Resources.ITexture Texture { get; set; }
	}
}
