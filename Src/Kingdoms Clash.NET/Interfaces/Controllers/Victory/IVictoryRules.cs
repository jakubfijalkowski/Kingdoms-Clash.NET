namespace Kingdoms_Clash.NET.Interfaces.Controllers.Victory
{
	/// <summary>
	/// Zasady wygranej.
	/// </summary>
	public interface IVictoryRules
	{
		/// <summary>
		/// Stan gry.
		/// </summary>
		IGameState GameState { get; set; }

		/// <summary>
		/// Sprawdza, czy któryś z graczy nie wygrał.
		/// </summary>
		/// <returns>Zwraca <see cref="Player.PlayerType.First"/>, gdy wygrał pierwszy gracz,<see cref="Player.PlayerType.Second"/>, gdy wygrał drugi lub
		/// <see cref="Player.PlayerType.Spectator"/> gdy nikt nie wygrał.</returns>
		Player.PlayerType Check();
	}
}
