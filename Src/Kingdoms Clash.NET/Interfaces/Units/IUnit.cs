using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Player;

	/// <summary>
	/// Bazowy interfejs jednostki.
	/// </summary>
	/// <remarks>
	/// Wszystkie atrybuty/statystyki jednostki powinny być implementowane przez IAttribute i atrybuty encji.
	/// Umożliwi to dostęp do nich z poziomu komponentów.
	/// Jedynie życie jednostki jest wyeksponowane na zewnątrz, ale też powinno być tak zaimplementowane.
	/// </remarks>
	public interface IUnit
		: IGameEntity
	{
		/// <summary>
		/// Opis(typ/identyfikator) jednostki.
		/// </summary>
		IUnitDescription Description { get; }

		/// <summary>
		/// Właściciel jednostki.
		/// </summary>
		IPlayer Owner { get; }

		/// <summary>
		/// Życie.
		/// </summary>
		int Health { get; set; }

		/// <summary>
		/// Sprawdza czy jednostka posiada komponent o takim typie i, jeśli tak, zwraca go.
		/// </summary>
		/// <typeparam name="T">Typ komponentu.</typeparam>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		IUnitComponent GetUnitComponent<T>()
			where T : IUnitComponent;
	}
}
