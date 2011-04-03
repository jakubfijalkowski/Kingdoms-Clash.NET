using System.Collections.Generic;
using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;
	using NET.Interfaces;
	using NET.Interfaces.Controllers;
	using NET.Interfaces.Map;
	using NET.Interfaces.Player;
	using NET.Interfaces.Units;

	/// <summary>
	/// Stan gry multiplayer.
	/// </summary>
	public class Multiplayer
		: IMultiplayer
	{
		#region Private Fields
		/// <summary>
		/// Manager encji "stałych" - graczy, kamery i mapy.
		/// </summary>
		private IEntitiesManager StaticEntities;

		/// <summary>
		/// Manager encji gry - jednostki, zasoby.
		/// </summary>
		private IEntitiesManager Entities;

		/// <summary>
		/// Jednostki czekające na usunięcie.
		/// </summary>
		private List<IGameEntity> ToRemove = new List<IGameEntity>();

		/// <summary>
		/// Kontrolery graczy.
		/// </summary>
		private IPlayerController[] PlayerControllers = new IPlayerController[2];
		#endregion

		#region IGameState Members
		#region Properties
		/// <summary>
		/// Ustawienia gry.
		/// </summary>
		public IGameSettings Settings { get; private set; }

		/// <summary>
		/// Tablica dwóch, aktualnie grających, graczy.
		/// </summary>
		public IPlayer[] Players { get; private set; }

		/// <summary>
		/// Mapa.
		/// </summary>
		public IMap Map { get; private set; }

		/// <summary>
		/// Kontroler(tryb) gry.
		/// </summary>
		public IGameController Controller { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje stan gry.
		/// </summary>
		/// <param name="settings">Ustawienia gry.</param>
		public void Initialize(IGameSettings settings)
		{
			//this.Settings = settings;
			//this.Players = new IPlayer[]
			//{
			//    new Player.Player(settings.PlayerA.Name, settings.PlayerA.Nation, 100),
			//    new Player.Player(settings.PlayerB.Name, settings.PlayerB.Nation, 100)
			//};
			//this.PlayerControllers[0] = settings.PlayerA.Controller;
			//this.PlayerControllers[1] = settings.PlayerB.Controller;
			//this.Map = settings.Map;
			//this.Controller = settings.Controller;

			//this.PlayerControllers[0].ShowStatistics = false;
			//this.PlayerControllers[1].ShowStatistics = false;
		}

		/// <summary>
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		public void Reset()
		{
			this.Controller.Reset();
			this.Entities.Clear();
		}

		/// <summary>
		/// Dodaje jednostkę do gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		public void Add(IUnit unit)
		{
			//Dla ułatwienia zarządzania jednostkami sprawdzamy kolizje jednostek tylko dla pierwszego gracza,
			//gdyż i tak gracz nie koliduje ze swoimi jednostkami a co za tym idzie może kolidować tylko z
			//jednostkami przeciwnika.
			if (unit.Owner.Type == PlayerType.First)
			{
				unit.CollisionWithUnit += this.Controller.HandleCollision;
			}
			unit.CollisionWithPlayer += this.Controller.HandleCollision;
			unit.CollisionWithResource += this.Controller.HandleCollision;

			this.Entities.Add(unit);
		}

		/// <summary>
		/// Dodaje zasób do gry.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		public void Add(IResourceOnMap resource)
		{
			resource.GameState = this;
			this.Entities.Add(resource);
		}

		/// <summary>
		/// Usuwa jednostkę z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		public void Remove(IUnit unit)
		{
			this.ToRemove.Add(unit);
		}

		/// <summary>
		/// Usuwa zasób z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		public void Remove(IResourceOnMap resource)
		{
			this.ToRemove.Add(resource);
		}
		#endregion
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje grę.
		/// </summary>
		/// <param name="gameInfo"></param>
		public Multiplayer(IGameInfo gameInfo)
		{
			this.Entities = new ClashEngine.NET.EntitiesManager.EntitiesManager(gameInfo);
			this.StaticEntities = new ClashEngine.NET.EntitiesManager.EntitiesManager(gameInfo);
		}
		#endregion

		#region IMultiplayer Members
		/// <summary>
		/// Uruchamia grę.
		/// </summary>
		public void Start()
		{

		}

		/// <summary>
		/// Kończy grę.
		/// </summary>
		public void Stop()
		{

		}
		#endregion
	}
}
