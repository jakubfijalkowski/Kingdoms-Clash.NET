using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Data
{
	/// <summary>
	/// Kontekst danych.
	/// </summary>
	public interface IDataContext
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Kontekst danych.
		/// </summary>
		object DataContext { get; }
	}
}
