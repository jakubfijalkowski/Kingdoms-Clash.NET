using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Data
{
	public interface IDataContext
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Kontekst danych.
		/// </summary>
		object DataContext { get; set; }
	}
}
