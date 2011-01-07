using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	using Data;

	/// <summary>
	/// Prosty wyzwalacz - przypisanie.
	/// </summary>
	public interface ISet
		: ITrigger
	{
		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		object Object { get; set; }

		/// <summary>
		/// Ścieżka, do której przypisana zostanie wartość.
		/// </summary>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		IPropertyPath Path { get; set; }

		/// <summary>
		/// Wartość do przypisania.
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
