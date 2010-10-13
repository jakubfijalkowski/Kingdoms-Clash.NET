using System;
using System.Collections.Generic;
using ClashEngine.NET.Cameras;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.ScreensManager;

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
		/// <summary>
		/// Jednostki czekające na usunięcie.
		/// </summary>
		private List<IGameEntity> ToRemove = new List<IGameEntity>();

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
			this.Controller.GameState = this;

			this.Players[0].GameState = this;
			this.Players[0].Type = PlayerType.First;

			this.Players[1].GameState = this;
			this.Players[1].Type = PlayerType.Second;

			this.Entities.Add(new OrthoCamera(
				new System.Drawing.RectangleF(0f, 0f, this.Map.Size.X, Math.Max(this.Map.Size.Y + Configuration.Instance.MapMargin, Configuration.Instance.ScreenSize.Y)),
				Configuration.Instance.ScreenSize,
				Configuration.Instance.CameraSpeed,
				true));

			this.Entities.Add(this.Map);
			this.Entities.Add(this.Players[0]);
			this.Entities.Add(this.Players[1]);

			//Od tej chwili to kontroler jest odpowiedzialny za wszystko.
			this.Controller.OnGameStarted();
		}

		public override void Update(double delta)
		{
			this.Controller.Update(delta);

			foreach (var ent in this.ToRemove)
			{
				this.Entities.Remove(ent);
			}
			this.ToRemove.Clear();
			base.Update(delta);
		}

		public override void Render()
		{
			OpenTK.Graphics.OpenGL.GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit);
			base.Render();
		}
		#endregion

		public SinglePlayer()
			: base("GameScreen", ClashEngine.NET.Interfaces.ScreensManager.ScreenType.Fullscreen)
		{ }
	}
}
