using System.Drawing;
namespace ClashEngine.NET.Interfaces.Components
{
	/// <summary>
	/// Sprite - komponent prezentujący teksturę w miejscu encji gry.
	/// 
	/// Wymagane atrybuty komponentu:
	/// PointF Position - pozycja
	/// SizeF Size - rozmiar
	/// float Rotation - rotacja
	/// </summary>
	public interface ISprite
		: EntitiesManager.IRenderableComponent
	{
		/// <summary>
		/// Tekstura sprite'u.
		/// </summary>
		public Resources.ITexture Texture { get; }

		/// <summary>
		/// Koordynaty tekstury. Domyślnie tekstura pokrywa cały sprite.
		/// </summary>
		public RectangleF TextureCoordinates { get; set; }
	}
}
