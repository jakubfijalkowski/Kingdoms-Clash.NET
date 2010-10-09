using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Kolekcja opisów komponentów jednostek.
	/// W kolekcji nie mogą znajdować się dwa opisy o takim samym typie.
	/// </summary>
	public interface IUnitComponentDescriptionsCollection
		: ICollection<IUnitComponentDescription>
	{
		/// <summary>
		/// Pobiera listę opisów o typie dziedziczącym ze wskazanego.
		/// </summary>
		/// <param name="type">Typ.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		/// <returns>Lista komponentów.</returns>
		IEnumerable<IUnitComponentDescription> Get(Type type);

		/// <summary>
		/// Pobiera listę opisów o typie dziedziczącym ze wskazanego.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <returns>Lista komponentów.</returns>
		IEnumerable<T> Get<T>()
			where T : IUnitComponentDescription;

		/// <summary>
		/// Pobiera opis komponentu o wskazanym typie.
		/// </summary>
		/// <param name="type">Typ.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		IUnitComponentDescription GetSingle(Type type);

		/// <summary>
		/// Pobiera opis komponentu o wskazanym typie.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		T GetSingle<T>()
			where T : class, IUnitComponentDescription;
	}
}
