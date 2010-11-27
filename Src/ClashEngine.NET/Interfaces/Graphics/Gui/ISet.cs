using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
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
		/// Właściwość, której zostanie przypisana dana wartość.
		/// </summary>
		string PropertyName { get; set; }

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
