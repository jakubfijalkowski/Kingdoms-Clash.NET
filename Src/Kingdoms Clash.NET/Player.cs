using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using OpenTK;

namespace Kingdoms_Clash.NET
{
	class Player
		: GameEntity
	{
		IAttribute<Vector2> Position;

		public Player()
			: base("Player")
		{

		}

		public override void InitEntity()
		{
			this.AddComponent(new Sprite("PlayerShip", ResourcesManager.Instance.Load<Texture>("PlayerShip.png")));

			this.GetOrCreateAttribute<Vector2>("Size").Value = new Vector2(105, 64);
			this.Position = this.GetOrCreateAttribute<Vector2>("Position");
			this.Position.Value = new Vector2((800.0f - 105.0f) / 2, 600.0f - 64.0f);
		}
	}
}
