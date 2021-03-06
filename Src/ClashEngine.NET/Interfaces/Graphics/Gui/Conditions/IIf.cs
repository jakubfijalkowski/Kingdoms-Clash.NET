﻿using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	using Data;

	/// <summary>
	/// Operator do porónywania wartości w warunku.
	/// </summary>
	public enum OperatorType
	{
		/// <summary>
		/// Równe(==).
		/// </summary>
		Equals,

		/// <summary>
		/// Nierówne(!=).
		/// </summary>
		NotEquals,

		/// <summary>
		/// Bitowe "i" - wykorzystywane przy flagach bitowych((Value & sth) != 0).
		/// </summary>
		And
	}

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

		/// <summary>
		/// Operator porównania.
		/// </summary>
		OperatorType Operator { get; set; }

		/// <summary>
		/// Wyzwalacze wywoływane gdy warunek nie jest(ale był!) spełniony.
		/// </summary>
		Conditions.ITriggersCollection Else { get; }
	}
}
