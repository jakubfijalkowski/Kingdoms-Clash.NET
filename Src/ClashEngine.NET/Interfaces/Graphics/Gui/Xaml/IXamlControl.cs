using System.ComponentModel;
using ClashEngine.NET.Converters;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Xaml
{
	/// <summary>
	/// Kontrolka, która może zostać zserializowana do XAML.
	/// </summary>
	public interface IXamlControl
		: IControl
	{
		/// <summary>
		/// Pozycja kontrolki.
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		Vector2 Position { get; set; }
	}
}
