using System;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Interfaces.Resources;
using ClashEngine.NET.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ClashEngine.NET;

namespace Kingdoms_Clash.NET
{
	public class Game
		: ClashEngine.NET.Game
	{
		const double MovingSpeed = 500.0;

		private static NLog.Logger Logger = NLog.LogManager.GetLogger("Kingdoms Clash.NET");

		private IGameEntity Character;
		private IShaderProgram MainShader;

		public Game()
			: base("Pierwsze okno stworzone za pomocą ClashEngine.NET!", 800, 600, false)
		{ }

		public override void Init()
		{
#if !DEBUG
			ClashEngine.NET.SystemInformation si = ClashEngine.NET.SystemInformation.Instance;
			Logger.Info("System information:");
			Logger.Info("OS: {0}, 64-bit: {1}", si.System.ToString(), si.Is64BitOS);
			Logger.Info("CLR version: {0}", si.CLRVersion.ToString());
			Logger.Info("RAM: {0}", si.MemorySize);
			Logger.Info("Processor: {0}, {1} MHz, {2} cores, {3}", si.ProcessorName, si.ProcessorSpeed, si.NumberOfCores, si.ProcessorArchitecture.ToString());
			Logger.Info("Graphics card: {0}, memory: {1}, driver version: {2}", si.GraphicsCardName, si.VRAMSize, si.GraphicsDriverVersion);
			Logger.Info("OpenGL version: {0}, GLSL version: {1}", si.OpenGLVersion, si.GLSLVersion);
			Logger.Info("Supported extensions: {0}", si.Extensions);
#endif
			this.ResourcesManager.ContentDirectory = "./Content";

			this.Character = new GameEntity("Character");
			this.Character.AddComponent(new Sprite("CharacterSprite", this.ResourcesManager.Load<Texture>("SampleSprite.png")));
			this.Character.GetOrCreateAttribute<Vector2>("Position").Value = new Vector2(this.Width / 2.0f, this.Height / 2.0f);
			this.Character.GetOrCreateAttribute<Vector2>("Size").Value = new Vector2(100.0f, 100.0f);

			this.MainShader = this.ResourcesManager.Load<ShaderProgram>("SampleShader");

			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0.0, this.Width, this.Height, 0.0, 0.0, 1.0);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();

			GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

			base.Init();
		}

		public override void Update(double delta)
		{
			var pos = this.Character.GetOrCreateAttribute<Vector2>("Position");
			//Poruszanie
			if (this.Keyboard[OpenTK.Input.Key.Up])
			{
				pos.Value = new Vector2(pos.Value.X, pos.Value.Y - (float)(MovingSpeed * delta));
			}
			if (this.Keyboard[OpenTK.Input.Key.Down])
			{
				pos.Value = new Vector2(pos.Value.X, pos.Value.Y + (float)(MovingSpeed * delta));
			}
			if (this.Keyboard[OpenTK.Input.Key.Left])
			{
				pos.Value = new Vector2(pos.Value.X - (float)(MovingSpeed * delta), pos.Value.Y);
			}
			if (this.Keyboard[OpenTK.Input.Key.Right])
			{
				pos.Value = new Vector2(pos.Value.X + (float)(MovingSpeed * delta), pos.Value.Y);
			}
			if (this.Keyboard[OpenTK.Input.Key.Escape])
			{
				this.Exit();
			}

			//Wyliczamy kąt rotacji - nie mam pojęcia czy to najlepsze rozwiązanie, matematyka u mnie kuleje :|
			float a = this.Mouse.Y - pos.Value.Y;
			float c = (new Vector2(this.Mouse.X , this.Mouse.Y) - pos.Value).Length;
			float angle = 0.0f;
			if (this.Mouse.X < pos.Value.X)
			{
				a = -a;
				angle -= MathHelper.Pi / 2;
			}
			else
			{
				angle += MathHelper.Pi / 2;
			}
			angle += (float)Math.Asin(a / c);

			this.Character.GetOrCreateAttribute<float>("Rotation").Value = angle;
			base.Update(delta);
		}

		public override void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.LoadIdentity();
			this.Character.Render();
			base.Render();
		}

		static void Main(string[] args)
		{
			using (var game = new Game())
			{
				game.Run();
			}
		}
	}
}
