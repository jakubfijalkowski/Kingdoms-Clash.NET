using System.Collections.Generic;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Player
{
	using Resources;
	using Units;

	/// <summary>
	/// Typ gracza.
	/// </summary>
	public enum PlayerType
		: int
	{
		/// <summary>
		/// Pierwszy gracz - po lewo.
		/// </summary>
		First = 0,

		/// <summary>
		/// Drugi gracz - po prawo.
		/// </summary>
		Second = 1,

		/// <summary>
		/// Obserwator.
		/// </summary>
		Observer = 0x7FFFFFFF
	}

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
		/// Stan gry do której należy gracz.
		/// </summary>
		IGameState GameState { get; set; }

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
		IResourcesCollection Resources { get; }

		/// <summary>
		/// Zdrowie bohatera(jego zamku).
		/// Powinno być zaimplementowane na bazie IAttribute.
		/// </summary>
		int Health { get; set; }

		/// <summary>
		/// Typ gracza.
		/// </summary>
		PlayerType Type { get; set; }
	}
}
