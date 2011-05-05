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

	/// <summary>
	/// Stan gry multiplayer.
	/// </summary>
	public class MultiplayerGameState
		: IGameState
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

		/// <summary>
		/// Licznik identyfikatorów.
		/// </summary>
		private uint[] Ids = new uint[3] { 0, 0, 0 };
		
		private IMultiplayer Game;
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

		#region Constructors
		/// <summary>
		/// Inicjalizuje grę.
		/// </summary>
		/// <param name="mp">Główny obiekt gry.</param>
		public MultiplayerGameState(IMultiplayer mp)
		{
			this.Game = mp;
			this.Entities = new ClashEngine.NET.EntitiesManager.EntitiesManager(mp.GameInfo);
			this.StaticEntities = new ClashEngine.NET.EntitiesManager.EntitiesManager(mp.GameInfo);
		}
		#endregion
	}
}
