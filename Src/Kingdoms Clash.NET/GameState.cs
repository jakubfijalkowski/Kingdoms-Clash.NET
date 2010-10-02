using System;
using ClashEngine.NET.ScreensManager;
using Kingdoms_Clash.NET.Interfaces;
using Kingdoms_Clash.NET.Player;
using Kingdoms_Clash.NET.Units;
using Kingdoms_Clash.NET.Units.Components;
using OpenTK.Graphics.OpenGL;
using Kingdoms_Clash.NET.Interfaces.Units;

namespace Kingdoms_Clash.NET
{
	class GameState
		: Screen, IGameState
	{
		private static readonly Nation TestNation = new Nation("Test", new UnitDescription[] { });
		private static readonly Human PlayerA = new Human(0, "PlayerA", TestNation);
		private static readonly Human PlayerB = new Human(1, "PlayerB", TestNation);

		private IUnit UnitA;
		private IUnit UnitB;

		#region IGameState Members

		public Interfaces.Player.IPlayer[] Players
		{
			get { throw new NotImplementedException(); }
		}

		public Interfaces.Map.IMap Map
		{
			get { throw new NotImplementedException(); }
		}

		public Interfaces.Controllers.IGameController Controller
		{
			get { throw new NotImplementedException(); }
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		#endregion

		public override void OnInit()
		{
			ClashEngine.NET.PhysicsManager.PhysicsManager.Instance.Gravity = new OpenTK.Vector2(0f, Configuration.Instance.Gravity);

			this.Entities.Add(new ClashEngine.NET.Cameras.OrthoCamera(new System.Drawing.RectangleF(0f, 0f, 200f, 75f), new OpenTK.Vector2(100f, 75f), 100f, true));
			this.Entities.Add(new Kingdoms_Clash.NET.Maps.DefaultMap());

			var ud = new UnitDescription("Test", 100, 5f, 5f);
			ud.Components.Add(new Movable());
			ud.Components.Add(new Sprite());
			(ud.Attributes as UnitAttributesCollection).Add(new UnitAttribute<float>("Velocity", 10f));
			(ud.Attributes as UnitAttributesCollection).Add(new UnitAttribute("Image", "NonExisting"));
			this.Entities.Add(this.UnitA = new Unit(ud, PlayerA));
			this.UnitA.Collide = (a, b) =>
			{
				if (a == b)
				{
				}
			};
		}

		public override void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			base.Render();
		}

		bool added = false;
		double accumulator = 0;
		public override void Update(double delta)
		{
			accumulator += delta;
			if (accumulator > 2 && !added)
			{
				added = true;

				var ud2 = new UnitDescription("Test", 100, 5f, 5f);
				ud2.Components.Add(new Movable());
				ud2.Components.Add(new Sprite());
				(ud2.Attributes as UnitAttributesCollection).Add(new UnitAttribute<float>("Velocity", 15f));
				(ud2.Attributes as UnitAttributesCollection).Add(new UnitAttribute("Image", "NonExisting"));
				this.Entities.Add(this.UnitB = new Unit(ud2, PlayerB));
			}
			base.Update(delta);
		}
	}
}
