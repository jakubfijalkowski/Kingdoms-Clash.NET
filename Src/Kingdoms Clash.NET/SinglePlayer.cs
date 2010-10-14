using System;
using System.Collections.Generic;
using ClashEngine.NET.Cameras;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.ScreensManager;
using OpenTK.Graphics.OpenGL;

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
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		#region Private Fields
		/// <summary>
		/// Manager encji "stałych" - graczy, kamery i mapy.
		/// </summary>
		private IEntitiesManager StaticEntities = new ClashEngine.NET.EntitiesManager.EntitiesManager();

		/// <summary>
		/// Jednostki czekające na usunięcie.
		/// </summary>
		private List<IGameEntity> ToRemove = new List<IGameEntity>();

		/// <summary>
		/// Czas od ostatniego dodania zasobu.
		/// </summary>
		private float ResourceRenewalAccumulator = 0f;
		#endregion

		#region IGameState Members
		#region Properties
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
		/// <param name="playerA">Pierwszy gracz.</param>
		/// <param name="playerB">Drugi gracz.</param>
		public void Initialize(IPlayer playerA, IPlayer playerB, IMap map, IGameController controller)
		{
			this.Players = new IPlayer[] { playerA, playerB };
			this.Map = map;
			this.Controller = controller;
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
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			this.Controller.GameState = this;

			this.Players[0].GameState = this;
			this.Players[0].Type = PlayerType.First;

			this.Players[1].GameState = this;
			this.Players[1].Type = PlayerType.Second;

			this.StaticEntities.Add(new OrthoCamera(
				new System.Drawing.RectangleF(0f, 0f, this.Map.Size.X, Math.Max(this.Map.Size.Y + Configuration.Instance.MapMargin, Configuration.Instance.ScreenSize.Y)),
				Configuration.Instance.ScreenSize,
				Configuration.Instance.CameraSpeed,
				true));

			this.StaticEntities.Add(this.Map);
			this.StaticEntities.Add(this.Players[0]);
			this.StaticEntities.Add(this.Players[1]);

			//Od tej chwili to kontroler jest odpowiedzialny za wszystko.
			this.Controller.OnGameStarted();
		}

		public override void Update(double delta)
		{
			this.HandleVictory();
			this.Controller.Update(delta);

			this.HandleResources(delta);

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
			GL.Clear(ClearBufferMask.ColorBufferBit);
			this.StaticEntities.Render();
			base.Render();
		}

		#region Events
		/// <summary>
		/// Po naciśnięciu R resetuje grę.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public override bool KeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			if (e.Key == OpenTK.Input.Key.R)
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
			: base("GameScreen", ClashEngine.NET.Interfaces.ScreensManager.ScreenType.Fullscreen)
		{ }
		#endregion

		#region Private Members
		/// <summary>
		/// Obsługuje zasoby.
		/// </summary>
		private void HandleResources(double delta)
		{
			this.ResourceRenewalAccumulator += (float)delta;
			if (this.ResourceRenewalAccumulator > Configuration.Instance.ResourceRenewalTime)
			{
				var res = this.Controller.RequestNewResource("wood");
				if (res != null)
				{
					this.Add(res);
				}
				this.ResourceRenewalAccumulator -= Configuration.Instance.ResourceRenewalTime;
			}
		}

		/// <summary>
		/// Sprawdza, czy ktoś nie wygrał.
		/// </summary>
		private void HandleVictory()
		{
			if (this.Players[0].Health <= 0)
			{
				Logger.Error("User {0} has won the match!", this.Players[1].Name);
				throw new NotSupportedException();
			}
			else if (this.Players[1].Health <= 0)
			{
				Logger.Error("User {0} has won the match!", this.Players[0].Name);
				throw new NotSupportedException();
			}
		}
		#endregion
	}
}
