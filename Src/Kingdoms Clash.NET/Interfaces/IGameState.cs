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
		/// Ustawienia dla kontrolera.
		/// </summary>
		IGameplaySettings Settings { get; }

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
		void Kill(IUnit unit);

		/// <summary>
		/// Usuwa zasób z gry.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		/// <param name="by">Jednostka, która zebrała.</param>
		void Gather(IResourceOnMap resource, IUnit by);
		#endregion
	}

#if !SERVER
	/// <summary>
	/// Stan gry przy jednym komputerze(niekoniecznie jednego gracza rzeczywistego).
	/// </summary>
	public interface ISingleplayer
	{
		/// <summary>
		/// Inicjalizuje stan gry.
		/// </summary>
		/// <param name="settings">Ustawienia gry.</param>
		void Initialize(ISingleplayerSettings settings);
	}

	/// <summary>
	/// Stan gry przez sieć.
	/// </summary>
	public interface IMultiplayer
	{
		/// <summary>
		/// Inicjalizuje stan gry.
		/// </summary>
		/// <param name="settings">Ustawienia gry.</param>
		void Initialize(IMultiplayerSettings settings);
	}

	/// <summary>
	/// Ekran stanu gry.
	/// </summary>
	public interface IGameStateScreen
		: IGameState, IScreen
	{ }
#endif
}
