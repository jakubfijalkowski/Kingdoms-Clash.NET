using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	using Data;

	/// <summary>
	/// Prosty warunek - jeśli coś jest równe czemuś ustaw coś na daną wartość.
	/// </summary>
	public interface IIf
		: ICondition
	{
		/// <summary>
		/// Obiekt, z którego pobrana będzie wartość.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		object Object { get; set; }

		/// <summary>
		/// Ścieżka, do której przypisana zostanie wartość.
		/// </summary>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		IPropertyPath Path { get; set; }

		/// <summary>
		/// Wartość do porównywania.
		/// </summary>
		object Value { get; set; }

		/// <summary>
		/// Konwerter dla wartości.
		/// </summary>
		[DefaultValue(null)]
		[TypeConverter(typeof(Converters.SystemTypeConverter))]
		Type CustomConverter { get; set; }
	}
}
