using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Player;

	/// <summary>
	/// Bazowy interfejs jednostki.
	/// </summary>
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

		#region Statistics
		/// <summary>
		/// Życie.
		/// Powinno być zaimplementowane na bazie IAttribute.
		/// </summary>
		int Health { get; set; }

		/// <summary>
		/// Szybkość jednostki.
		/// Powinno być zaimplementowane na bazie IAttribute.
		/// </summary>
		int Speed { get; }
		#endregion
	}
}
