using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using ClashEngine.NET.ScreensManager;
using OpenTK;

namespace Kingdoms_Clash.NET
{
	class GameOverScreen
		: Screen
	{
		public GameOverScreen()
		{
			this.IsFullscreen = true;
			this.Entities.AddEntity(new GameOverEntity());
		}
	}

	class GameOverEntity
		: GameEntity
	{
		public GameOverEntity()
			: base("GameOver")
		{ }

		public override void InitEntity()
		{
			this.AddComponent(new Sprite("GameOverImage", ResourcesManager.Instance.Load<Texture>("GameOver.png")));
			this.GetOrCreateAttribute<Vector2>("Size").Value = new Vector2(800, 600);
		}
	}
}
