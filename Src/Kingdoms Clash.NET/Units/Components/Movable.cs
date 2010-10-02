using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Utilities;
using FarseerPhysics.Dynamics;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Komponent jednostki określający, że jednostka potrafi się poruszać.
	/// </summary>
	public class Movable
		: Component, IMovable
	{
		private IAttribute<OpenTK.Vector2> Velocity_;
		private IAttribute<Body> Body;

		/// <summary>
		/// Prędkość jednostki.
		/// </summary>
		public OpenTK.Vector2 Velocity
		{
			get { return this.Velocity_.Value; }
			set { this.Velocity_.Value = value; }
		}

		public Movable()
			: base("Movable")
		{ }

		/// <summary>
		/// Inicjalizuje komponent ustawiając prędkość pobraną z atrybutów jednostki.
		/// </summary>
		/// <param name="owner"></param>
		public override void OnInit()
		{
			this.Velocity_ = this.Owner.Attributes.GetOrCreate<OpenTK.Vector2>("Velocity");
			this.Velocity = new OpenTK.Vector2((this.Owner as IUnit).Description.Attributes.Get<float>("Velocity"), 0f);

			this.Body = this.Owner.Attributes.Get<Body>("Body");
		}

		/// <summary>
		/// Przemieszcza jednostkę.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.Body.Value.Position += ((float)delta * this.Velocity).ToXNA();
		}

		#region ICloneable Members
		public object Clone()
		{
			return new Movable();
		}
		#endregion
	}
}
