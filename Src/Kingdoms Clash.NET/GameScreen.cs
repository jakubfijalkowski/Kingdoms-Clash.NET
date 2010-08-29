using System.Collections.Generic;
using ClashEngine.NET.ScreensManager;

namespace Kingdoms_Clash.NET
{
	class GameScreen
		: Screen
	{
		const int MaxEnemies = 3;

		List<Enemy> Enemies = new List<Enemy>();
		Player Player;

		public GameScreen()
		{
			this.IsFullscreen = true;
			this.Entities.AddEntity(this.Player = new Player());
		}
	}
}
