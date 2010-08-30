using System;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using OpenTK;

namespace Kingdoms_Clash.NET
{
	public class Enemy
		: GameEntity
	{
		public static readonly Vector2 Size = new Vector2(94/2, 90/2);

		static readonly Random Random = new Random();

		const int MinX = 20;
		const int MaxX = 780 - 90;

		const double MovingSpeed = 150.0;

		public IAttribute<Vector2> Position { get; private set; }

		public Enemy()
			: base("Enemy")
		{ }

		public override void InitEntity()
		{
			this.AddComponent(new Sprite("EnemyShip", ResourcesManager.Instance.Load<Texture>("EnemyShip.png")));

			this.GetOrCreateAttribute<Vector2>("Size").Value = Size;
			this.Position = this.GetOrCreateAttribute<Vector2>("Position");
			this.Position.Value = new Vector2(Random.Next(MinX, MaxX), 0);
		}

		public override void Update(double delta)
		{
			this.Position.Value = new Vector2(this.Position.Value.X, this.Position.Value.Y + (float)(MovingSpeed * delta));
		}
	}
}
