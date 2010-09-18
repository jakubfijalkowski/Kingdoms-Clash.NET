using ClashEngine.NET.Interfaces.ScreensManager;

namespace Kingdoms_Clash.NET.Interfaces
{
	using Controllers;
	using Map;
	using Player;

	/// <summary>
	/// Ekran(stan) gry.
	/// </summary>
	public interface IGameState
		: IScreen
	{
		#region Properties
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
		#endregion

		#region Methods
		/// <summary>
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		void Reset();
		#endregion
	}
}
