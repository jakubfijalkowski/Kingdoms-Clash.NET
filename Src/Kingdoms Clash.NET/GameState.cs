using ClashEngine.NET.Interfaces.ScreensManager;
using ClashEngine.NET.ScreensManager;
using ClashEngine.NET.Cameras;
using OpenTK.Graphics.OpenGL;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;

	using Maps;
	using System.Drawing;

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

			this.Camera = new OrthoCamera(new RectangleF(0.0f, 0.0f, this.Map.Width, this.Map.Height + Configuration.Instance.MapMargin),
				Configuration.Instance.ScreenSize, Configuration.Instance.CameraSpeed, true);

			this.Entities.AddEntity(this.Map);
			this.Entities.AddEntity(this.Camera);

			GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
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
