using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Kolekcja opisów jednostek.
	/// </summary>
	public interface IUnitDescriptionsCollection
		: ICollection<IUnitDescription>
	{
		/// <summary>
		/// Wyszukuje jednostki na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Opis jednostki bądź null, gdy nie znaleziono.</returns>
		IUnitDescription this[string id] { get; }

		/// <summary>
		/// Pobiera informacje o jednostce na wskazanej pozycji w liście.
		/// </summary>
		/// <param name="idx">Indeks.</param>
		/// <returns></returns>
		IUnitDescription this[int idx] { get; }

		/// <summary>
		/// Wyszukuje jednostki na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Opis jednostki bądź null, gdy nie znaleziono.</returns>
		IUnitDescription Find(string id);
	}
}
