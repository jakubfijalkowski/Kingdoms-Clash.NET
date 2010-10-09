using ClashEngine.NET;
using ClashEngine.NET.Utilities;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Units;
	using Units;
	using Units.Components;

	/// <summary>
	/// Główna klasa gry.
	/// </summary>
	public class KingdomsClashNetGame
		: Game, IGame
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		
		#region IGame Members
		public IGameStateScreen Game { get; private set; }

		public IMenuState Menu
		{
			get { throw new System.NotImplementedException(); }
		}
		#endregion

		#region Constructors
		public KingdomsClashNetGame()
			: base("Kingdom's Clash.NET",
					Configuration.Instance.WindowSize.Width, Configuration.Instance.WindowSize.Height,
					Configuration.Instance.Fullscreen,
#if DEBUG
 false
#else
			true
#endif
)
		{ }
		#endregion

		#region Game Members
		public override void Init()
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

			this.ResourcesManager.ContentDirectory = "Content";

			IUnitDescription testUnit = new UnitDescription("TestUnit", 10, 2f, 3f);

			//testUnit.Attributes.Add(new UnitAttribute<string>("Image", "TestUnit.png"));
			//testUnit.Attributes.Add(new UnitAttribute<float>("Velocity", 10f));
			//testUnit.Attributes.Add(new UnitAttribute<int>("Strength", 10));

			//testUnit.Components.Add(new Movable());
			//testUnit.Components.Add(new Sprite());
			//testUnit.Components.Add(new ContactSoldier());

			testUnit.Costs.Add("wood", 10); //Testowy koszt jednostki

			INation testNation = new Units.Nation("TestNation", "Castle.png",
				new IUnitDescription[]
				{
					testUnit
				});

			this.Game = new SinglePlayer();
			this.Game.Initialize(new Player.KeyboardControlledPlayer("A", testNation, OpenTK.Input.Key.A),
				new Player.KeyboardControlledPlayer("B", testNation, OpenTK.Input.Key.L),
				new Maps.DefaultMap(),
				new Controllers.ClassicGame());

			this.ScreensManager.AddAndActivate(new FPSCounter() { LogStatistics = 10.0f });
			this.ScreensManager.AddAndActivate(this.Game);
			//this.ScreensManager.AddAndMakeActive(this.Menu);
			base.Init();
		}

		public override void Update(double delta)
		{
#if DEBUG
			//Dodatkowe skróty na wyjście z gry.
			if (this.Input.Keyboard[OpenTK.Input.Key.Escape] || //Escape
				((this.Input.Keyboard[OpenTK.Input.Key.AltLeft] || this.Input.Keyboard[OpenTK.Input.Key.AltRight]) && this.Input.Keyboard[OpenTK.Input.Key.F4])) //Alt + F4
			{
				this.Exit();
			}
#endif

			base.Update(delta);
		}
		#endregion

		#region Main
		static void Main(string[] args)
		{
			try
			{
				using (var game = new KingdomsClashNetGame())
				{
					game.Run();
				}
			}
			catch (System.Exception ex)
			{
				Logger.Fatal("Fatal error: {0}", ex.Message);
				Logger.Fatal("Stack trace: {0}", ex.StackTrace);
			}
		}
		#endregion
	}
}
