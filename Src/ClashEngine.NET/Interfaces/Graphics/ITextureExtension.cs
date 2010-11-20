namespace ClashEngine.NET.Interfaces.Graphics
{
	/// <summary>
	/// Interfejs dla rozszerzenie XAML - tekstura.
	/// </summary>
	public interface ITextureExtension
	{
		/// <summary>
		/// Ścieżka do tekstury.
		/// </summary>
		string Path { get; set; }

		/// <summary>
		/// Jeśli jest niepuste określa Id w atlasie tekstur.
		/// </summary>
		string TextureId { get; set; }
	}
}
