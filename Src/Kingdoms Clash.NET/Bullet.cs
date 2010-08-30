using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using OpenTK;

namespace Kingdoms_Clash.NET
{
	class Bullet
		: GameEntity
	{
		public static Vector2 Size = new Vector2(10/2, 32/2);

		const double MoveSpeed = 500.0f;

		public IAttribute<Vector2> Position;

		public Bullet(float startPos)
			: base("Bullet")
		{
			//Tworzenie atrybutu pozycju przeniesione z InitEntity do konstruktora, ponieważ muszę wiedzieć, gdzie jest statek ;)
			this.Position = this.GetOrCreateAttribute<Vector2>("Position");
			this.Position.Value = new Vector2(startPos, 600.0f - 64.0f - Size.Y);
		}

		public override void InitEntity()
		{
			this.AddComponent(new Sprite("BulletSprite", ResourcesManager.Instance.Load<Texture>("PlayerBullet.png")));

			this.GetOrCreateAttribute<Vector2>("Size").Value = Size;
		}

		public override void Update(double delta)
		{
			this.Position.Value = new Vector2(this.Position.Value.X, this.Position.Value.Y - (float)(MoveSpeed * delta));
			base.Update(delta);
		}
	}
}
