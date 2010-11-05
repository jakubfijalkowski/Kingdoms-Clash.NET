using System.Drawing;

namespace ClashEngine.NET.Interfaces.Components
{
	/// <summary>
	/// Sprite - duszek - komponent prezentujący teksturę w miejscu encji gry.
	/// </summary>
	public interface ISprite
		: EntitiesManager.IRenderableComponent
	{
		/// <summary>
		/// Tekstura sprite'u.
		/// </summary>
		Resources.ITexture Texture { get; }

		/// <summary>
		/// Koordynaty tekstury. Domyślnie tekstura pokrywa cały sprite.
		/// </summary>
		RectangleF TextureCoordinates { get; set; }

		/// <summary>
		/// Odbija teskturę sprite'a w poziomie.
		/// </summary>
		void FlipHorizontal();

		/// <summary>
		/// Odbija teskturę sprite'a w pionie.
		/// </summary>
		void FlipVertical();
	}
}
