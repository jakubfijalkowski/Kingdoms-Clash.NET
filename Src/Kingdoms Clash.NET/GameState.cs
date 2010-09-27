using System.Drawing;
using ClashEngine.NET.Cameras;
using ClashEngine.NET.Interfaces.ScreensManager;
using ClashEngine.NET.ScreensManager;
using OpenTK.Graphics.OpenGL;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;

	using Maps;

	/// <summary>
	/// Główny stan(ekran) gry.
	/// </summary>
	public class GameState
		: Screen, IGameState
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Properties
		/// <summary>
		/// Kamera.
		/// </summary>
		private OrthoCamera Camera { get; set; }
		#endregion

		#region IGameState Members
		/// <summary>
		/// Tablica dwóch, aktualnie grających, graczy.
		/// TODO: zaimplementować graczy.
		/// </summary>
		public IPlayer[] Players
		{
			get { throw new System.NotImplementedException(); }
		}

		/// <summary>
		/// Mapa.
		/// </summary>
		public IMap Map { get; private set; }

		/// <summary>
		/// Kontroler(tryb) gry.
		/// </summary>
		public IGameController Controller { get; private set; }

		/// <summary>
		/// Resetuje stan gry(zaczyna ją od nowa).
		/// </summary>
		public void Reset()
		{
			this.Map.Reset();
			this.Controller.Reset();
		}
		#endregion

		public GameState()
		{
			//Ustawianie właściowści ekranu
			base.IsFullscreen = true;

			Logger.Debug("Creating default game state");
			this.Map = new DefaultMap();
			this.Controller = new Controllers.NormalGame();
			ClashEngine.NET.PhysicsManager.PhysicsManager.Instance.Gravity = new OpenTK.Vector2(0f, Configuration.Instance.Gravity);

			this.Camera = new OrthoCamera(new RectangleF(0.0f, 0.0f, this.Map.Width, this.Map.Height + Configuration.Instance.MapMargin),
				Configuration.Instance.ScreenSize, Configuration.Instance.CameraSpeed, true);

			this.Entities.Add(this.Map);
			this.Entities.Add(this.Camera);
			
			GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

			Kingdoms_Clash.NET.Units.Sample.SampleNation sn = new Units.Sample.SampleNation();
			var unit = sn.CreateUnit(sn.AvailableUnits[0], null);
			this.Entities.Add(unit);
		}

		/// <summary>
		/// Obsługa zmiany stanu.
		/// Jeśli ekran został otwarty na nowo musimy zresetować stan gry.
		/// </summary>
		public override void StateChanged(ScreenState oldState)
		{
			if (this.State != ScreenState.Closed && oldState == ScreenState.Closed)
			{
				this.Reset();
			}
		}

		public override void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			base.Render();
		}
	}
}
