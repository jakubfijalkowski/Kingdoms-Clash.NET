using System.Collections.Generic;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Player
{
	using Resources;
	using Units;

	/// <summary>
	/// Bazowy interfejs dla gracza.
	/// </summary>
	public interface IPlayer
		: IGameEntity
	{
		/// <summary>
		/// Nazwa gracza.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Nacja gracza.
		/// </summary>
		INation Nation { get; }

		/// <summary>
		/// Jednostki gracza.
		/// </summary>
		IList<IUnit> Units { get; }

		/// <summary>
		/// Zasoby użytkownika.
		/// </summary>
		IList<IResource> Resources { get; }

		/// <summary>
		/// Zdrowie bohatera(jego zamku).
		/// Powinno być zaimplementowane na bazie IAttribute.
		/// </summary>
		int Health { get; }
	}
}
