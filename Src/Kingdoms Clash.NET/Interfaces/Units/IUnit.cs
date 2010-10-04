using ClashEngine.NET.Interfaces.EntitiesManager;
using OpenTK;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Player;

	/// <summary>
	/// Handler dla zdarzenia kolizji jednostek.
	/// </summary>
	/// <param name="a">Pierwsza jednostka.</param>
	/// <param name="b">Druga jednostka.</param>
	public delegate void UnitCollideEventHandler(IUnit a, IUnit b);

	/// <summary>
	/// Bazowy interfejs jednostki.
	/// </summary>
	/// <remarks>
	/// Wszystkie atrybuty/statystyki jednostki powinny być implementowane przez IAttribute i atrybuty encji.
	/// Umożliwi to dostęp do nich z poziomu komponentów.
	/// Jedynie życie jednostki i pozycja są wyeksponowane na zewnątrz, ale też powinny być tak zaimplementowane.
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
		/// Pozycja jednostki.
		/// </summary>
		Vector2 Position { get; }

		/// <summary>
		/// Zdarzenie kolizji jednostek.
		/// </summary>
		event UnitCollideEventHandler Collide;
	}
}
