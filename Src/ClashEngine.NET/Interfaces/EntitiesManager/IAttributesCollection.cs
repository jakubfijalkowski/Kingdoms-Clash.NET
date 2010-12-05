using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Kolekcja atrybutów encji.
	/// </summary>
	public interface IAttributesCollection
		: ICollection<IAttribute>
	{
		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Atrybut.</returns>
		IAttribute this[string id] { get; }

		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <typeparam name="T">Typ atrybutu.</typeparam>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		IAttribute<T> Get<T>(string id);

		/// <summary>
		/// Wyszukuje albo tworzy atrybut o podanym ID i typie.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Atrybut.</returns>
		IAttribute GetOrCreate(string id);

		/// <summary>
		/// Wyszukuje albo tworzy atrybut o podanym ID i typie.
		/// </summary>
		/// <typeparam name="T">Wymagany typ atrybutu.</typeparam>
		/// <param name="id">Identyfikator.</param>
		/// <exception cref="System.InvalidCastException">Rzucane gdy atrybut istnieje ale ma inny typ niż rządany.</exception>
		/// <returns>Atrybut.</returns>
		IAttribute<T> GetOrCreate<T>(string id);

		/// <summary>
		/// Podmienia atrybut o podanym id na wskazany.
		/// Gdy atrybut nie istnieje - dodaje go.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="with">Atrybut zamieniany.</param>
		void Replace(string id, IAttribute with);
	}
}
