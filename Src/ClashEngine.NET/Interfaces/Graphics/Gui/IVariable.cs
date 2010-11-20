using System;
using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Zmienna XAML.
	/// </summary>
	public interface IVariable
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Identyfikator.
		/// </summary>
		string Id { get; }
		
		/// <summary>
		/// Wartość.
		/// </summary>
		object Value { get; set; }

		/// <summary>
		/// Wymagany typ obiektu.
		/// </summary>
		Type RequiredType { get; }

		/// <summary>
		/// Konwerter wskazany przez użytkownika.
		/// </summary>
		Type CustomConverter { get; }
	}
}
