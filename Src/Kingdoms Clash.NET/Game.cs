using ClashEngine.NET;
using ClashEngine.NET.Utilities;

namespace Kingdoms_Clash.NET
{
	using Interfaces;

	/// <summary>
	/// Główna klasa gry.
	/// </summary>
	public class KingdomsClashNetGame
		: Game, IGame
	{
		#region IGame Members
		public IGameState Game
		{
			get { throw new System.NotImplementedException(); }
		}

		public IMenuState Menu
		{
			get { throw new System.NotImplementedException(); }
		}
		#endregion

		public KingdomsClashNetGame()
			: base("Kingdom's Clash.NET",
					Configuration.Instance.WindowWidth, Configuration.Instance.WindowHeight,
					Configuration.Instance.Fullscreen, true)
		{ }

		public override void Init()
		{
			//this.ScreensManager.AddAndMakeActive(this.Game);
			//this.ScreensManager.AddAndMakeActive(this.Menu);
			this.ScreensManager.AddAndMakeActive(new FPSCounter() { LogStatistics = 10.0f });
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
