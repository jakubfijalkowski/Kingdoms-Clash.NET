using System.ComponentModel;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Obiekt renderera - tekst.
	/// </summary>
	public interface IText
		: IObject
	{
		/// <summary>
		/// Wartość tekstu.
		/// </summary>
		string Content { get; }

		/// <summary>
		/// Pozycja obiektu.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		Vector2 Size { get; set; }
	}
}
