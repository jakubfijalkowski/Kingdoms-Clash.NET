using System;
using System.Collections.Generic;
using ClashEngine.NET;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Stan-ekran gry przy jednym komputerze(niekoniecznie jednego gracza rzeczywistego).
	/// </summary>
	public class SinglePlayer
		: Screen, IGameState, IGameStateScreen
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Private Fields
		/// <summary>
		/// Manager encji "stałych" - graczy, kamery i mapy.
		/// </summary>
		private IEntitiesManager StaticEntities;

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
			this.Settings = settings;
			this.Players = new IPlayer[]
			{
				new Player.Player(settings.PlayerA.Name, settings.PlayerA.Nation, 100),
				new Player.Player(settings.PlayerB.Name, settings.PlayerB.Nation, 100)
			};
			this.PlayerControllers[0] = settings.PlayerA.Controller;
			this.PlayerControllers[1] = settings.PlayerB.Controller;
			this.Map = settings.Map;
			this.Controller = settings.Controller;

			this.PlayerControllers[0].ShowStatistics = settings.PlayerA.ShowStatistics;
			this.PlayerControllers[1].ShowStatistics = settings.PlayerB.ShowStatistics;
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

		#region Screen Members
		public override void OnInit()
		{
			Logger.Info("Starting game with controller {0} and players {1} and {2}", this.Settings.Controller.GetType().Name, this.Players[0].Name, this.Players[1].Name);

			base.OnInit();
			this.StaticEntities = new ClashEngine.NET.EntitiesManager.EntitiesManager(this.GameInfo);

			this.Controller.GameState = this;
			this.Settings.VictoryRules.GameState = this;

			this.Controller.PreGameStarted();

			this.Players[0].Type = PlayerType.First;
			this.PlayerControllers[0].Player = this.Players[0];
			this.PlayerControllers[0].GameState = this;
			this.PlayerControllers[0].Initialize(this.OwnerManager, this.GameInfo.MainWindow.Input);

			this.Players[1].Type = PlayerType.Second;
			this.PlayerControllers[1].Player = this.Players[1];
			this.PlayerControllers[1].GameState = this;
			this.PlayerControllers[1].Initialize(this.OwnerManager, this.GameInfo.MainWindow.Input);

			float h = NET.Settings.ScreenSize * (Configuration.Instance.WindowSize.Height / (float)Configuration.Instance.WindowSize.Width);

			var cam = new ClashEngine.NET.Graphics.Cameras.Movable2DCamera(new OpenTK.Vector2(NET.Settings.ScreenSize, h),
				new System.Drawing.RectangleF(0f, 0f, this.Map.Size.X, Math.Max(this.Map.Size.Y + NET.Settings.MapMargin, h)));
			this.Camera = cam;
			this.StaticEntities.Add(cam.GetCameraEntity(Configuration.Instance.CameraSpeed));

			this.StaticEntities.Add(this.Map);
			this.StaticEntities.Add(new Player.PlayerEntity(this.Players[0], this));
			this.StaticEntities.Add(new Player.PlayerEntity(this.Players[1], this));

			//Fizyka
			PhysicsManager.Instance.World.AddController(new Internals.VelocityLimit());

			//Od tej chwili to kontroler jest odpowiedzialny za wszystko.
			this.Controller.GameStarted();
			Logger.Info("Game started");
		}

		public override void Update(double delta)
		{
			if (this.HandleInput())
			{
				return;
			}
			this.HandleVictory();
			this.Controller.Update(delta);

			foreach (var ent in this.ToRemove)
			{
				this.Entities.Remove(ent);
			}
			this.ToRemove.Clear();

			this.StaticEntities.Update(delta);
			base.Update(delta);
		}

		public override void Render()
		{
			base.Render();
			this.StaticEntities.Render();
		}

		#region Events
		/// <summary>
		/// Po naciśnięciu R resetuje grę.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private bool HandleInput()
		{
			if (this.GameInfo.MainWindow.Input[OpenTK.Input.Key.R])
			{
				this.Reset();
				return true;
			}
			return false;
		}
		#endregion
		#endregion

		#region Constructors
		public SinglePlayer()
			: base("GameScreen", ClashEngine.NET.Interfaces.ScreenType.Fullscreen)
		{ }
		#endregion

		#region Private Members
		/// <summary>
		/// Sprawdza, czy ktoś nie wygrał.
		/// </summary>
		private void HandleVictory()
		{
			var winner = this.Settings.VictoryRules.Check();
			AdditionalScreens.WinnerScreen winnerScreen = null;
			switch (winner)
			{
				case PlayerType.First:
					Logger.Error("User {0} has won the match!", this.Players[0].Name);
					winnerScreen = this.OwnerManager["WinnerScreen"] as AdditionalScreens.WinnerScreen;
					winnerScreen.ChangeWinner(false);
					winnerScreen.MoveToFront();
					winnerScreen.Activate();
					break;

				case PlayerType.Second:
					Logger.Error("User {0} has won the match!", this.Players[1].Name);
					winnerScreen = this.OwnerManager["WinnerScreen"] as AdditionalScreens.WinnerScreen;
					winnerScreen.ChangeWinner(true);
					winnerScreen.MoveToFront();
					winnerScreen.Activate();
					break;
			}
		}
		#endregion
	}
}