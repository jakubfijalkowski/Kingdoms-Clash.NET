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
		/// Inicjalizuje stan gry.
		/// </summary>
		/// <param name="playerA">Pierwszy gracz.</param>
		/// <param name="playerB">Drugi gracz.</param>
		/// <param name="map">Mapa.</param>
		/// <param name="controller">Kontroler gry.</param>
		void Initialize(IPlayer playerA, IPlayer playerB, IMap map, IGameController controller);

		/// <summary>
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		void Reset();

		/// <summary>
		/// Dodaje jednostkę do gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		void AddUnit(IUnit unit);
		
		/// <summary>
		/// Usuwa jednostkę z gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		void RemoveUnit(IUnit unit);
		#endregion
	}

	/// <summary>
	/// Ekran stanu gry.
	/// </summary>
	public interface IGameStateScreen
		: IGameState, IScreen
	{ }
}
