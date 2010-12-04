using System.Collections.Generic;

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
	/// Gracz - podstawowe informacje o nim.
	/// </summary>
	public interface IPlayer
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
		/// Zasoby, które gracz aktualnie posiada.
		/// </summary>
		IResourcesCollection Resources { get; }

		/// <summary>
		/// Zdrowie bohatera(jego zamku).
		/// </summary>
		int Health { get; set; }

		/// <summary>
		/// Typ gracza.
		/// </summary>
		PlayerType Type { get; set; }
	}
}
