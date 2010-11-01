using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla kolekcji komponentów.
	/// Musi ustawiać właściwości Owner i Input.
	/// </summary>
	public interface IComponentsCollection
		: ICollection<IComponent>
	{
		/// <summary>
		/// Lista komponentów które potrafią się renderować.
		/// </summary>
		IRenderableComponentsCollection RenderableComponents { get; }

		/// <summary>
		/// Pobiera komponent o wskazanym id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		IComponent this[string id] { get; }

		/// <summary>
		/// Pobiera listę komponentów o wskazanym typie.
		/// </summary>
		/// <typeparam name="T">Typ komponentów.</typeparam>
		/// <returns>Lista.</returns>
		IEnumerable<T> Get<T>()
			where T : IComponent;

		/// <summary>
		/// Pobiera listę komponentów o wskazanym typie.
		/// </summary>
		/// <param name="componentType">Typ komponentu.</param>
		/// <returns>Lista.</returns>
		IEnumerable<IComponent> Get(Type componentType);

		/// <summary>
		/// Pobiera pierwszy komponent o wskazanym typie.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		T GetSingle<T>()
			where T : IComponent;

		/// <summary>
		/// Pobiera pierwszy komponent o wskazanym typie.
		/// </summary>
		/// <param name="componentType">Typ komponentu.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		IComponent GetSingle(Type componentType);

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera komponent o wskazanym id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>True, jeśli zawiera, w przeciwnym razie false.</returns>
		bool Contains(string id);

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera komponent o wskazanym type.
		/// </summary>
		/// <typeparam name="T">Typ komponentu.</typeparam>
		/// <returns>True, jeśli zawiera, w przeciwnym razie false</returns>
		bool Contains<T>()
			where T : IComponent;

		/// <summary>
		/// Usuwa komponenty o wskazanym id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Ilość usuniętych komponentów.</returns>
		int RemoveAll(string id);
	}
}
