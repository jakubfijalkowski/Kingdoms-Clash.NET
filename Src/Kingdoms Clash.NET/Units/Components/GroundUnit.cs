using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units.Components;

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
			get	{ return new OpenTK.Vector2(); }
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
