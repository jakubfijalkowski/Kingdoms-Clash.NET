using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	using Data;

	/// <summary>
	/// Zwiększa wartość.
	/// </summary>
	public interface IIncrease
		: ITrigger
	{
		/// <summary>
		/// Obiekt docelowy.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		object Object { get; set; }

		/// <summary>
		/// Ścieżka.
		/// </summary>
		[TypeConverter(typeof(Converters.PropertyPathConverter))]
		IPropertyPath Path { get; set; }

		/// <summary>
		/// Wartość o jaką zostanie zwiększony.
		/// </summary>
		object Amount { get; set; }

		/// <summary>
		/// Konwerter dla wartości.
		/// </summary>
		[DefaultValue(null)]
		[TypeConverter(typeof(Converters.SystemTypeConverter))]
		Type CustomConverter { get; set; }
	}
}
