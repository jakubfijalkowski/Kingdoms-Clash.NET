using ClashEngine.NET.Interfaces;

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
		/// Ustawienia gry.
		/// </summary>
		IGameSettings Settings { get; }

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
		/// <param name="settings">Ustawienia gry.</param>
		void Initialize(IGameSettings settings);

		/// <summary>
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		void Reset();

		/// <summary>
		/// Dodaje jednostkę do gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		void Add(IUnit unit);

		/// <summary>
		/// Dodaje zasób jako encje.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		void Add(IResourceOnMap resource);
		
		/// <summary>
		/// Usuwa jednostkę z gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		void Remove(IUnit unit);

		/// <summary>
		/// Usuwa zasób z gry.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		void Remove(IResourceOnMap resource);
		#endregion
	}
#if !SERVER
	/// <summary>
	/// Ekran stanu gry.
	/// </summary>
	public interface IGameStateScreen
		: IGameState, IScreen
	{ }
#endif
}
