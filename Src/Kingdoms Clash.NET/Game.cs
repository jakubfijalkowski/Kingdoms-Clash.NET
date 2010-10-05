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
		#region IGame Members
		public IGameStateScreen Game { get; private set; }

		public IMenuState Menu
		{
			get { throw new System.NotImplementedException(); }
		}
		#endregion

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

		public override void Init()
		{
			this.ResourcesManager.ContentDirectory = "Content";

			IUnitDescription testUnit = new UnitDescription("TestUnit", 10, 2f, 3f);

			testUnit.Attributes.Add(new UnitAttribute<string>("Image", "TestUnit.png"));
			testUnit.Attributes.Add(new UnitAttribute<float>("Velocity", 10f));
			testUnit.Attributes.Add(new UnitAttribute<int>("Strength", 10));

			testUnit.Components.Add(new Movable());
			testUnit.Components.Add(new Sprite());
			testUnit.Components.Add(new ContactSoldier());

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

			this.ScreensManager.AddAndMakeActive(new FPSCounter() { LogStatistics = 10.0f });
			this.ScreensManager.AddAndMakeActive(this.Game);
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

		#region Main
		static void Main(string[] args)
		{
			using (var game = new KingdomsClashNetGame())
			{
				game.Run();
			}
		}
		#endregion
	}
}
