using System.Collections.Generic;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;
	using NET.Interfaces;
	using NET.Interfaces.Controllers;
	using NET.Interfaces.Map;
	using NET.Interfaces.Player;
	using NET.Interfaces.Units;
	using NET.Interfaces.Controllers.Victory;

	/// <summary>
	/// Stan gry multiplayer.
	/// </summary>
	public class MultiplayerGameState
		: IGameState
	{
		#region Private Fields
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		
		/// <summary>
		/// Manager encji gry - jednostki, zasoby.
		/// </summary>
		private IEntitiesManager Entities;

		/// <summary>
		/// Jednostki czekające na usunięcie.
		/// </summary>
		private List<IGameEntity> ToRemove = new List<IGameEntity>();

		/// <summary>
		/// Licznik identyfikatorów.
		/// </summary>
		private uint[] Ids = new uint[3] { 0, 0, 0 };

		private IMultiplayer Game;
		private IVictoryRules VictoryRules;
		#endregion

		#region IGameState Members
		#region Properties
		/// <summary>
		/// Ustawienia gry.
		/// </summary>
		public IGameplaySettings Settings { get; private set; }

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
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		public void Reset()
		{
			this.Controller.Reset();
			this.Entities.Clear();
			//this.StaticEntities.Clear();
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

			unit.UnitId = this.Ids[(int)unit.Owner.Type]++;

			this.Entities.Add(unit);
			this.Game.UnitAdded(unit);
		}

		/// <summary>
		/// Dodaje zasób do gry.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		public void Add(IResourceOnMap resource)
		{
			resource.GameState = this;
			resource.ResourceId = this.Ids[2]++;
			this.Entities.Add(resource);
			this.Game.ResourceAdded(resource);
		}

		/// <summary>
		/// Usuwa jednostkę z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		public void Kill(IUnit unit)
		{
			this.ToRemove.Add(unit);
			this.Game.UnitDestroyed(unit);
		}

		/// <summary>
		/// Usuwa zasób z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		/// <param name="by">Jednostka, która zebrała zasób.</param>
		public void Gather(IResourceOnMap resource, IUnit by)
		{
			this.ToRemove.Add(resource);
			this.Game.ResourceRemoved(resource, by);
		}
		#endregion
		#endregion

		#region Internals
		internal void Initialize(IServerGameConfiguration cfg)
		{
			this.Players = new IPlayer[]
			{
				new Player.Player(cfg.PlayerA.Name, cfg.PlayerA.Nation, 100),
				new Player.Player(cfg.PlayerB.Name, cfg.PlayerB.Nation, 100)
			};

			this.Controller.PreGameStarted();

			this.Players[0].Type = PlayerType.First;
			this.Players[1].Type = PlayerType.Second;

			this.Entities.Add(this.Map);
			this.Entities.Add(new Player.PlayerEntity(this.Players[0], this));
			this.Entities.Add(new Player.PlayerEntity(this.Players[1], this));

			this.Controller.GameStarted();
		}

		public void Update(double delta)
		{
			this.HandleVictory();
			this.Controller.Update(delta);

			foreach (var ent in this.ToRemove)
			{
				this.Entities.Remove(ent);
			}
			this.ToRemove.Clear();

			this.Entities.Update(delta);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje grę.
		/// </summary>
		/// <param name="mp">Główny obiekt gry.</param>
		public MultiplayerGameState(IMultiplayer mp)
		{
			this.Game = mp;
			this.Entities = new ClashEngine.NET.EntitiesManager.EntitiesManager(mp.GameInfo);

			this.VictoryRules = System.Activator.CreateInstance(ServerConfiguration.Instance.VictoryRules) as IVictoryRules;
			this.Controller = System.Activator.CreateInstance(ServerConfiguration.Instance.GameController) as IGameController;
			this.Settings = ServerConfiguration.Instance.ControllerSettings;
			this.Map = new Maps.DefaultMap();
		}
		#endregion

		#region Private
		/// <summary>
		/// Sprawdza, czy ktoś nie wygrał.
		/// </summary>
		private void HandleVictory()
		{
			var winner = this.VictoryRules.Check();
			switch (winner)
			{
				case PlayerType.First:
					Logger.Error("User {0} has won the match!", this.Players[0].Name);
					break;

				case PlayerType.Second:
					Logger.Error("User {0} has won the match!", this.Players[1].Name);
					break;
			}
		}
		#endregion
	}
}
