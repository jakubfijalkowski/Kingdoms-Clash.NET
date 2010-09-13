using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Player
{
	using Units;

	/// <summary>
	/// Bazowy interfejs dla gracza.
	/// </summary>
	public interface IPlayer
		: IGameEntity
	{
		/// <summary>
		/// Nacja gracza.
		/// </summary>
		INation Nation { get; }
	}
}
