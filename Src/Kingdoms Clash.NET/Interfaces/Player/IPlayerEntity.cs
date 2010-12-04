using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Player
{
	/// <summary>
	/// Encja gracza - zamku.
	/// </summary>
	public interface IPlayerEntity
		: IGameEntity
	{
		/// <summary>
		/// Gracz do którego się odnosi.
		/// </summary>
		IPlayer Player { get; }

		/// <summary>
		/// Stan gry dla gracza.
		/// </summary>
		IGameState GameState { get; }
	}
}
