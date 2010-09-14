namespace Kingdoms_Clash.NET.Interfaces
{
	using Controllers;
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

		/// <summary>
		/// Kontroler(tryb) gry.
		/// </summary>
		IGameController Controller { get; }

		/// <summary>
		/// Ekran rozgrywki.
		/// </summary>
		IGameScreen GameScreen { get; }
	}
}
