using System.Collections.Generic;
using ClashEngine.NET;
using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Utilities;
using OpenTK.Graphics.OpenGL;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Units;

	/// <summary>
	/// Główna klasa gry.
	/// </summary>
	public class KingdomsClashNetGame
		: Game, IGame
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region IGame Members
		/// <summary>
		/// Ekran rozgrywki.
		/// </summary>
		public IGameStateScreen Game { get; private set; }

		/// <summary>
		/// Ekran głównego menu.
		/// </summary>
		public IMenuState Menu { get; private set; }

		/// <summary>
		/// Lista nacji.
		/// </summary>
		public IList<INation> Nations { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt gry.
		/// </summary>
		/// <param name="nations">List nacji</param>
		public KingdomsClashNetGame(IList<INation> nations)
			: base(Configuration.Instance.WindowSize.Width, Configuration.Instance.WindowSize.Height, "Kingdoms Clash.NET",
			WindowFlags.Default & (Configuration.Instance.Fullscreen ? ~WindowFlags.Fullscreen : ~WindowFlags.None)
			& (Configuration.Instance.VSync ? ~WindowFlags.VSync : ~WindowFlags.None))
		{
			if (nations == null)
			{
				throw new System.ArgumentNullException("nations");
			}
			this.Nations = nations;
		}
		#endregion

		#region Game Members
		public override void OnInit()
		{
#if !DEBUG
			Logger.Info("System information: ");
			var si = SystemInformation.Instance;
			Logger.Info("OS: {0}, CLR: {1}", si.System.ToString(), si.CLRVersion.ToString());
			Logger.Info("RAM size: {0}", si.MemorySize);
			Logger.Info("Processor: {0} {1}MHz, arch: {2}, cores: {3}", si.ProcessorName, si.ProcessorSpeed, si.ProcessorArchitecture.ToString(), si.NumberOfCores);
			Logger.Info("Graphics card: {0}, VRAM: {1}, drivers version: {2}", si.GraphicsCardName, si.VRAMSize, si.GraphicsDriverVersion);
			Logger.Info("OpenGL version: {0}, GLSL version: {1}", si.OpenGLVersion, si.GLSLVersion);
#endif
			if (SystemInformation.Instance.OpenGLVersion < new System.Version(1, 5))
			{
				Logger.Fatal("This game requires at least OpenGL 1.5");
				System.Windows.Forms.MessageBox.Show("This game requires at least OpenGL 1.5", "Error",
					System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				this.Exit();
				return;
			}

			this.SetGlobals();

			//INation nation1 = null, nation2 = null;
			//foreach (var nation in this.Nations)
			//{
			//    if (nation.Name == Configuration.Instance.Player1Nation)
			//    {
			//        nation1 = nation;
			//    }
			//    if (nation.Name == Configuration.Instance.Player2Nation)
			//    {
			//        nation2 = nation;
			//    }
			//}
			//if (nation1 == null || nation2 == null)
			//{
			//    Logger.Fatal("Cannot find nation for player 1 or 2");
			//    System.Windows.Forms.MessageBox.Show("Cannot find nation for player 1 or 2", "Error",
			//        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			//    this.Exit();
			//    return;
			//}

			//Player.Controllers.GuiControllerFactory factory = new Player.Controllers.GuiControllerFactory();
			//factory.GameInfo = this.Info;
			//var controlers = factory.Produce();

			//GameSettings settings = new GameSettings
			//{
			//    PlayerA = new Player.PlayerInfo("A", nation1, controlers[0], true),
			//    PlayerB = new Player.PlayerInfo("B", nation2, controlers[1], true),
			//    Map = new Maps.DefaultMap(),
			//    Controller = new Controllers.ClassicGame(),
			//    VictoryRules = new Controllers.Victory.KillerWins(),
			//    Gameplay = Controllers.ControllerSettingsAttribute.GetSettingsFor(typeof(Controllers.ClassicGame))
			//};

			//this.Game = new SinglePlayer();
			//(this.Game as SinglePlayer).Initialize(settings);

			MultiplayerSettings settings = new MultiplayerSettings
			{
				Address = System.Net.IPAddress.Loopback,
				Port = 12345,
				PlayerNick = "Test"
			}; //Testowe dane.

			this.Game = new Multiplayer();
			(this.Game as Multiplayer).Initialize(settings);

			if (Configuration.Instance.UseFPSCounter)
			{
				this.Info.Screens.AddAndActivate(
					new FPSCounter(this.Info.Content.Load<ClashEngine.NET.Graphics.Resources.SystemFont>("Arial,15,"),
					System.Drawing.Color.Yellow,
					new OpenTK.Vector2(Configuration.Instance.WindowSize.Width, Configuration.Instance.WindowSize.Height)
					));
			}
			this.Info.Screens.Add(new AdditionalScreens.WinnerScreen(this.Game));
			this.Info.Screens.AddAndActivate(this.Game);
			//this.ScreensManager.AddAndMakeActive(this.Menu);
		}

		public override void OnDeinit()
		{ }

		public override void OnRender()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			base.OnRender();
		}
		#endregion

		#region Private members
		/// <summary>
		/// Ustawia globalne zmienne.
		/// </summary>
		private void SetGlobals()
		{
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			this.Info.Content.ContentDirectory = Defaults.RootDirectory;
			ClashEngine.NET.PhysicsManager.Instance.Gravity = new OpenTK.Vector2(0f, Settings.Gravity);
		}
		#endregion

		#region Main
		static void Main(string[] args)
		{
			UserData.LoaderBase loader = new UserData.ClientLoader(Defaults.RootDirectory, Defaults.UserData);

			loader.LoadConfiguration();
			loader.LoadNations();
			loader.LoadResources();

			using (var game = new KingdomsClashNetGame(loader.Nations))
			{
				game.Run();
			}
		}
		#endregion
	}
}