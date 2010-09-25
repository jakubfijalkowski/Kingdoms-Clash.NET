using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Utilities;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units.Components;
	using Interfaces.Units;

	/// <summary>
	/// Komponent określający, czy jednostka jest naziemna czy nie.
	/// Wymaga, by był dodany po PhysicalObject.
	/// </summary>
	public class GroundUnit
		: Component, IGroundUnit
	{
		public GroundUnit()
			: base("GroundUnit")
		{ }

		#region Component Members
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			//TODO: do usunięcia!
			var body = this.Owner.Attributes.Get<FarseerPhysics.Dynamics.Body>("Body").Value;
			//body.IgnoreGravity = true;
			FarseerPhysics.Collision.Shapes.PolygonShape poly = new FarseerPhysics.Collision.Shapes.PolygonShape();
			poly.SetAsBox((this.Owner as IUnit).Description.GetAttribute<float>("ImageWidth") / 2f, (this.Owner as IUnit).Description.GetAttribute<float>("ImageHeight") / 2.0f);
			var f = body.CreateFixture(poly);
			f.Friction = 1.0f;
			body.SleepingAllowed = false;
			body.FixedRotation = true;
		}

		public override void Update(double delta)
		{
			var pos = this.Owner.Attributes.Get<OpenTK.Vector2>("Position");
			pos.Value += this.Velocity * (float)delta;
		}
		#endregion

		#region IGroundUnit Members
		/// <summary>
		/// Prędkość jednostki.
		/// </summary>
		public OpenTK.Vector2 Velocity
		{
			get	{ return new OpenTK.Vector2(0.2f, /*Configuration.Instance.Gravity*/0.0f); }
		}
		#endregion

		#region ICloneable Members
		public object Clone()
		{
			return new GroundUnit();
		}
		#endregion
	}
}
