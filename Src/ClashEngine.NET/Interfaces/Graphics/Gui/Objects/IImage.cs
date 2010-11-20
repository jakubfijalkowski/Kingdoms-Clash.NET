using System.ComponentModel;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	/// <summary>
	/// Obrazek.
	/// </summary>
	public interface IImage
		: IObject
	{
		/// <summary>
		/// Pozycja obrazka.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar obrazka.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		Vector2 Size { get; set; }
	}
}
