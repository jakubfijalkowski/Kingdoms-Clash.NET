using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Kolekcja komponentów.
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
