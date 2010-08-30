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
		public static readonly Vector2 Size = new Vector2(105/2, 64/2);

		const double MovingSpeed = 300.0;

		public IAttribute<Vector2> Position;

		public Player()
			: base("Player")
		{ }

		public override void InitEntity()
		{
			this.AddComponent(new Sprite("PlayerShip", ResourcesManager.Instance.Load<Texture>("PlayerShip.png")));

			this.GetOrCreateAttribute<Vector2>("Size").Value = Size;
			this.Position = this.GetOrCreateAttribute<Vector2>("Position");
			this.Position.Value = new Vector2((800.0f - Size.X) / 2, 600.0f - Size.Y);
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
			else if (newX > 800.0f - Size.X)
			{
				newX = 800.0f - Size.X;
			}
			this.Position.Value = new Vector2(newX, this.Position.Value.Y);

			base.Update(delta);
		}
	}
}
