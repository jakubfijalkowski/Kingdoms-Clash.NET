using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Resources
{
	/// <summary>
	/// Kolekcja zasobów.
	/// Nie powinna blokować dodawania zasobu o takim samym Id a jedynie dodawać wartość.
	/// 
	/// Klucz - identyfikator.
	/// Wartość - wartość zasobu.
	/// </summary>
	/// <seealso cref="Kingdoms_Clash.NET.Resources.ResourcesCollection"/>
	public interface IResourcesCollection
		: IDictionary<string, uint>
	{
		/// <summary>
		/// Usuwa z kolekcji podaną ilość zasobu.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <param name="value">Wartość.</param>
		/// <returns>Czy udało się usunąć.</returns>
		bool Remove(string id, uint value);

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wskazana ilość danego zasobu.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <param name="value">Żądana ilość.</param>
		/// <returns>Czy jest go wystarczająco.</returns>
		bool Contains(string id, uint value);
	}
}
