using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
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
		/// Właściwość.
		/// </summary>
		string PropertyName { get; set; }

		/// <summary>
		/// Wartość, do której będzie porównana.
		/// </summary>
		object Value { get; set; }

		/// <summary>
		/// Konwerter dla wartości.
		/// </summary>
		Type CustomConverter { get; set; }

		/// <summary>
		/// Typ wartości.
		/// </summary>
		Type ValueType { get; set; }
	}
}
