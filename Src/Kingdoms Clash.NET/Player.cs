using ClashEngine.NET;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using OpenTK;
using OpenTK.Input;

namespace Kingdoms_Clash.NET
{
	class Player
		: GameEntity
	{
		const double MovingSpeed = 300.0;

		IAttribute<Vector2> Position;

		public Player()
			: base("Player")
		{ }

		public override void InitEntity()
		{
			this.AddComponent(new Sprite("PlayerShip", ResourcesManager.Instance.Load<Texture>("PlayerShip.png")));

			this.GetOrCreateAttribute<Vector2>("Size").Value = new Vector2(105, 64);
			this.Position = this.GetOrCreateAttribute<Vector2>("Position");
			this.Position.Value = new Vector2((800.0f - 105.0f) / 2, 600.0f - 64.0f);
		}

		public override void Update(double delta)
		{
			float positionOffset = 0.0f;
			if (Input.Instance.Keyboard[Key.Left])
			{
				positionOffset -= (float)(MovingSpeed * delta);
			}
			if (Input.Instance.Keyboard[Key.Right])
			{
				positionOffset += (float)(MovingSpeed * delta);
			}

			float newX = this.Position.Value.X + positionOffset;
			if (newX < 0.0f)
			{
				newX = 0.0f;
			}
			else if (newX > 800.0f - 105.0f)
			{
				newX = 800.0f - 105.0f;
			}
			this.Position.Value = new Vector2(newX, this.Position.Value.Y);

			base.Update(delta);
		}
	}
}
