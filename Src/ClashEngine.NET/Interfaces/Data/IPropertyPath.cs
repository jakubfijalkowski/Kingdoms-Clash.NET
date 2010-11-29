using System;
using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Data
{
	/// <summary>
	/// Ścieżka do właściwości lub pola zapisana jako tekst.
	/// </summary>
	[TypeConverter(typeof(Converters.PropertyPathConverter))]
	public interface IPropertyPath
		: INotifyPropertyChanged, ISupportInitialize, IDisposable
	{
		/// <summary>
		/// Ścieżka jako tekst.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Obiekt główny.
		/// </summary>
		object Root { get; set; }

		/// <summary>
		/// Typ obiektu głównego.
		/// </summary>
		Type RootType { get; }

		/// <summary>
		/// Typ wartości.
		/// </summary>
		Type ValueType { get; }

		/// <summary>
		/// Konwerter typów dla wartości.
		/// </summary>
		TypeConverter ValueConverter { get; }

		/// <summary>
		/// Aktualna wartość.
		/// </summary>
		object Value { get; set; }

		/// <summary>
		/// Czy obiekt został zainicjalizowany.
		/// </summary>
		bool Initialized { get; }
	}
}
