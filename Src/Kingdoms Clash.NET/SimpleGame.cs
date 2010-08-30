using ClashEngine.NET;
using ClashEngine.NET.Utilities;
using OpenTK.Graphics.OpenGL;

namespace Kingdoms_Clash.NET
{
	/// <summary>
	/// Prosta gra zbudowana za pomocą ClashEngine.NET i OpenTK.
	/// </summary>
	class SimpleGame
		: Game
	{
		public SimpleGame()
			: base("SimpleGame.NET", 800, 600, false, false)
		{ }

		public override void Init()
		{
			this.ResourcesManager.ContentDirectory = "Content";

			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0.0, this.Width, this.Height, 0.0, 0.0, 1.0);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.ClearColor(0.0f, 0.3f, 0.0f, 1.0f);

			var screen = new GameScreen();
			this.ScreensManager.AddAndMakeActive(screen);
			this.ScreensManager.AddAndMakeActive(new FPSCounter() { LogStatistics = 20.0f });

			base.Init();
		}

		public override void Update(double delta)
		{
			if (this.Input.Keyboard[OpenTK.Input.Key.Escape])
			{
				this.Exit();
			}
			base.Update(delta);
		}

		public override void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			base.Render();
		}

		static void Main(string[] args)
		{
			using (SimpleGame game = new SimpleGame())
			{
				game.Run();
			}
		}
	}
}
