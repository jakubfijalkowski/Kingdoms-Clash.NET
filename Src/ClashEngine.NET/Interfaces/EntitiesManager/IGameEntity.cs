using System.Collections.ObjectModel;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla encji gry.
	/// </summary>
	public interface IGameEntity
	{
		#region Properties
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Manager encji.
		/// </summary>
		IEntitiesManager Manager { get; }

		/// <summary>
		/// Lista komponentów.
		/// </summary>
		ReadOnlyCollection<IComponent> Components { get; }

		/// <summary>
		/// Lista atrybutów.
		/// </summary>
		ReadOnlyCollection<IAttribute> Attributes { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Wywoływane przy inicjalizacji encji gry - dadaniu do managera.
		/// </summary>
		/// <param name="entitiesManager">Właściciel encji.</param>
		void Init(IEntitiesManager entitiesManager);

		/// <summary>
		/// Dodaje komponent do encji.
		/// </summary>
		/// <param name="component">Komponent. Musi być unikatowy.</param>
		void AddComponent(IComponent component);

		/// <summary>
		/// Dodaje atrybut do encji.
		/// </summary>
		/// <param name="attribute">Atrybut. Musi być unikatowy.</param>
		void AddAttribute(IAttribute attribute);

		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		IAttribute GetAttribute(string id);

		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <typeparam name="T">Typ atrybutu.</typeparam>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		IAttribute<T> GetAttribute<T>(string id);

		/// <summary>
		/// Wyszukuje albo tworzy atrybut o podanym ID i typie.
		/// </summary>
		/// <typeparam name="T">Wymagany typ atrybutu.</typeparam>
		/// <param name="id">Identyfikator.</param>
		/// <exception cref="System.InvalidCastException">Rzucane gdy atrybut istnieje ale ma inny typ niż rządany.</exception>
		/// <returns>Atrybut.</returns>
		IAttribute<T> GetOrCreateAttribute<T>(string id);

		/// <summary>
		/// Uaktualnia wszystkie komponenty.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Rysuje encję.
		/// </summary>
		void Render();
		#endregion
	}
}
