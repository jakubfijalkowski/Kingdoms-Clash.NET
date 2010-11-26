using System.ComponentModel;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	/// <summary>
	/// Typ rozciągania obrazka.
	/// </summary>
	public enum StretchType
	{
		/// <summary>
		/// Rozciąga na cały obszar.
		/// </summary>
		Fill,

		/// <summary>
		/// Powtarza obrazek tak, by wypełniał całą długość osi X.
		/// Rozciąga na osi Y.
		/// </summary>
		RepeatX,

		/// <summary>
		/// Powtarza obrazek tak, by wypełniał całą długość osi Y.
		/// Rozciąga na osi X.
		/// </summary>
		RepeatY,

		/// <summary>
		/// Powtarza obrazek tak, by wypełniał całą przestrzeń.
		/// </summary>
		Repeat
	}

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

		/// <summary>
		/// Typ rozciągania obrazka.
		/// </summary>
		StretchType Stretch { get; set; }
	}
}
