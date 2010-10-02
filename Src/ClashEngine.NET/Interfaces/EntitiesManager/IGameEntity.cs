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
		/// </summary>
		IComponentsCollection Components { get; }

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
		/// Uaktualnia wszystkie komponenty.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Rysuje encję.
		/// </summary>
		void Render();
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji encji(dodaniu do managera).
		/// </summary>
		void OnInit();

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji encji(usunięcie z managera).
		/// </summary>
		void OnDeinit();
		#endregion
	}
}
