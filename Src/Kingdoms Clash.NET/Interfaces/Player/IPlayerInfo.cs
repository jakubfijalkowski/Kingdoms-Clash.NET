namespace Kingdoms_Clash.NET.Interfaces.Player
{
	/// <summary>
	/// Informacje o graczu przekazywane do <see cref="IGameState.Initialize"/>.
	/// </summary>
	public interface IPlayerInfo
	{
		/// <summary>
		/// Nazwa gracza.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Wybrana nacja.
		/// </summary>
		Units.INation Nation { get; }

		/// <summary>
		/// Kontroler.
		/// </summary>
		IPlayerController Controller { get; }
	}
}
