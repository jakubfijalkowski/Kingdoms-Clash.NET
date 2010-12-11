using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	/// <summary>
	/// Prostokąt.
	/// </summary>
	public interface IRectangle
		: IObject
	{
		/// <summary>
		/// Rozmiar.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Jego kolor.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector4Converter))]
		OpenTK.Vector4 Color { get; set; }
	}
}
