using System.ComponentModel;
using ClashEngine.NET.Converters;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Xaml
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface IButton
		: IXamlControl
	{
		/// <summary>
		/// Pozycja kontrolki.
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		Vector2 Size { get; set; }

		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}
