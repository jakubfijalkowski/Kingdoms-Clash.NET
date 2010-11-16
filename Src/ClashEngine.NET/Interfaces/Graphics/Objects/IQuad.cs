using System.ComponentModel;
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
		[TypeConverter(typeof(Converters.Vector2Converter))]
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar prostokąta.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		Vector2 Size { get; set; }

		/// <summary>
		/// Kolor prostokąta.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector4Converter))]
		Vector4 Color { get; set; }
	}
}
