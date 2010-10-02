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
		/// Identyfikator gracza.
		/// Nie może istnieć wielu graczy(aktualnie grających!) o tym samym ID.
		/// Maksymalna wartość: 32.
		/// </summary>
		byte PlayerID { get; }

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
		/// Zasoby, które gracz aktualnie posiada.
		/// </summary>
		IList<IResource> Resources { get; }

		/// <summary>
		/// Zdrowie bohatera(jego zamku).
		/// Powinno być zaimplementowane na bazie IAttribute.
		/// </summary>
		int Health { get; set; }
	}
}
