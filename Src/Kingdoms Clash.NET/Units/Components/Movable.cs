using System.Drawing;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using OpenTK;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units.Components;
	using Kingdoms_Clash.NET.Interfaces.Units;

	/// <summary>
	/// Komponent dający jednostce możliwość poruszania się.
	/// </summary>
	/// <remarks>
	/// Użyte atrybuty opisu jednostki:
	/// float Speed
	/// </remarks>
	public class Movable
		: Component, IMovable
	{
		private IAttribute<float> Speed_;
		private IAttribute<Vector2> Position;

		#region IMovable Members
		/// <summary>
		/// Szybkość poruszania się jednostki.
		/// </summary>
		public float Speed
		{
			get { return this.Speed_.Value; }
			private set { this.Speed_.Value = value; }
		}
		#endregion

		public Movable()
			: base("Movable")
		{ }

		/// <summary>
		/// Pobiera atrybuty i ustawia odpowiednie wartości atrybutów.
		/// </summary>
		/// <param name="owner"></param>
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			this.Speed_ = this.Owner.GetOrCreateAttribute<float>("Speed");
			this.Position = this.Owner.GetOrCreateAttribute<Vector2>("Position");
			this.Speed = (this.Owner as IUnit).Description.GetAttribute<float>("Speed");
		}

		public override void Update(double delta)
		{
			this.Position.Value = new Vector2((float)(this.Position.Value.X + delta * this.Speed), this.Position.Value.Y);
		}

		#region ICloneable Members
		object System.ICloneable.Clone()
		{
			return new Movable();
		}
		#endregion
	}
}
