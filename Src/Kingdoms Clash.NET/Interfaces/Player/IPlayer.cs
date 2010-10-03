using System.Collections.Generic;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Player
{
	using Resources;
	using Units;

	/// <summary>
	/// Zdarzenie kolizji jednostki z graczem.
	/// Kolizja odbywa się "awsze, niezależnie czy jednostka jest tego gracza, czy przeciwnika.
	/// </summary>
	/// <param name="unit">Jednostka, która koliduje.</param>
	/// <param name="player">Gracz z którym koliduje.</param>
	public delegate void UnitCollideWithPlayerEventHandler(IUnit unit, IPlayer player);

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
		IGameState GameState { get; }

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

		/// <summary>
		/// Typ gracza.
		/// </summary>
		PlayerType Type { get; set; }

		/// <summary>
		/// Zdarzenie kolizji jednostki z graczem.
		/// </summary>
		/// <seealso cref="UnitCollideWithPlayerEventHandler"/>
		event UnitCollideWithPlayerEventHandler Collide;
	}
}
