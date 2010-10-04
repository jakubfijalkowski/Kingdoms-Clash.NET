using ClashEngine.NET.Interfaces.ScreensManager;

namespace Kingdoms_Clash.NET.Interfaces
{
	using Controllers;
	using Map;
	using Player;
	using Units;

	/// <summary>
	/// Stan gry.
	/// </summary>
	public interface IGameState
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

		/// <summary>
		/// Dodaje jednostkę do gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		void AddUnit(IUnit unit);
		#endregion
	}

	/// <summary>
	/// Ekran stanu gry.
	/// </summary>
	public interface IGameStateScreen
		: IGameState, IScreen
	{ }
}
