using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kingdoms_Clash.NET.Controllers.Victory
{
	using Interfaces;
	using Interfaces.Controllers.Victory;

	/// <summary>
	/// Reguły wygranej - zabójca wygrywa.
	/// </summary>
	public class KillerWins
		: IKillerWins
	{
		#region IVictoryRules Members
		/// <summary>
		/// Stan gry.
		/// </summary>
		public IGameState GameState { get; set; }

		/// <summary>
		/// Sprawdza, czy któryś z graczy nie wygrał.
		/// </summary>
		public Interfaces.Player.PlayerType Check()
		{
			if (this.GameState.Players[0].Health <= 0)
				return Interfaces.Player.PlayerType.Second;
			else if (this.GameState.Players[1].Health <= 0)
				return Interfaces.Player.PlayerType.First;
			return Interfaces.Player.PlayerType.Spectator;
		}
		#endregion
	}
}
