namespace Kingdoms_Clash.NET.Interfaces.Player
{
	/// <summary>
	/// Kontroler gracza.
	/// </summary>
	public interface IPlayerController
	{
		/// <summary>
		/// Gracz, którym kontroler steruje.
		/// </summary>
		IPlayer Player { get; }

		/// <summary>
		/// Stan gry do której należy gracz.
		/// </summary>
		IGameState GameState { get; set; }
	}
}
