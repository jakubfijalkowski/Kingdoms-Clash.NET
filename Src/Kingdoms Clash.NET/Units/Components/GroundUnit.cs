using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Interfaces.PhysicsManager;
using ClashEngine.NET.PhysicsManager;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Komponent określający, czy jednostka jest naziemna czy nie.
	/// Wymaga, by był dodany po PhysicalObject.
	/// </summary>
	public class GroundUnit
		: Component, IGroundUnit
	{
		#region Privates
		private IVelocity Velocity_ = null;
		#endregion

		public GroundUnit()
			: base("GroundUnit")
		{ }

		#region Component Members
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			this.Velocity_ = new Velocity("Velocity", new OpenTK.Vector2((this.Owner as IUnit).Description.GetAttribute<float>("Speed"), 0.0f));

			IVelocitiesCollection velocities = this.Owner.GetOrCreateAttribute<IVelocitiesCollection>("Velocities").Value;
			velocities.Add(this.Velocity_);
		}

		public override void Update(double delta)
		{ }
		#endregion

		#region IGroundUnit Members
		/// <summary>
		/// Prędkość jednostki.
		/// </summary>
		public OpenTK.Vector2 Velocity
		{
			get	{ return this.Velocity_.Value; }
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
