using System;
using System.ComponentModel;

namespace ClashEngine.NET.Data.Internals
{
	/// <summary>
	/// Interfejs reprezentujący pojedynczy poziom w PropertyPath.
	/// </summary>
	internal interface IPropertyLevel
	{
		#region Properties
		/// <summary>
		/// Numer poziomu.
		/// </summary>
		int Level { get; }

		/// <summary>
		/// Nazwa właściwości danego poziomu.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Typ poziomu po pobraniu wartości.
		/// </summary>
		Type Type { get; }

		/// <summary>
		/// Wartość.
		/// </summary>
		object Value { get; }

		/// <summary>
		/// Metoda, która będzie wywoływana przy zmianie Value.
		/// Parametr: Level.
		/// </summary>
		Action<int> ValueChanged { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Uaktualnia wartość.
		/// </summary>
		/// <param name="root">Obiekt przed.</param>
		void UpdateValue(object root);

		/// <summary>
		/// Zmienia wartość dla danego obiektu.
		/// </summary>
		/// <param name="to">Obiekt.</param>
		/// <param name="value">Wartość.</param>
		void SetValue(object to, object value);

		/// <summary>
		/// Rejestruje, jeśli może, zdarzenie Value.PropertyChanged.
		/// </summary>
		/// <param name="root">Obiekt, do którego przynależy dana właściwość.</param>
		void RegisterPropertyChanged(object root);
		
		/// <summary>
		/// Usuwa, jeśli może, zdarzenie Value.PropertyChanged.
		/// </summary>
		/// <param name="root">Obiekt, do którego przynależy dana właściwość.</param>
		void UnregisterPropertyChanged(object root);

		/// <summary>
		/// Pobiera konwerter typów.
		/// </summary>
		/// <returns></returns>
		TypeConverter GetTypeConverter();
		#endregion
	}
}
