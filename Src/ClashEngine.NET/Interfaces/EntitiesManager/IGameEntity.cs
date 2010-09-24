using System.Collections.Generic;

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
		/// Zmieniać za pomocą odpowiednich metod, nie ręcznie!
		/// </summary>
		IList<IComponent> Components { get; }

		/// <summary>
		/// Kolekcja atrybutów.
		/// </summary>
		IAttributesCollection Attributes { get; }
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
