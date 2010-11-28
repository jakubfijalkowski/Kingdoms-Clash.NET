using System;
using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Data
{
	/// <summary>
	/// Ścieżka do właściwości lub pola zapisana jako tekst.
	/// </summary>
	public interface IPropertyPath
		: INotifyPropertyChanged, ISupportInitialize
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
		/// Aktualna wartość.
		/// </summary>
		object Value { get; set; }
	}
}
