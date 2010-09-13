namespace Kingdoms_Clash.NET.Interfaces
{
	using Map;
	using Player;

	/// <summary>
	/// Główny obiekt gry.
	/// </summary>
	public interface IGame
		: ClashEngine.NET.Interfaces.IGame
	{
		/// <summary>
		/// Tablica dwóch, aktualnie grających, graczy.
		/// </summary>
		IPlayer[] Players { get; }

		/// <summary>
		/// Mapa.
		/// </summary>
		IMap Map { get; }
	}
}
