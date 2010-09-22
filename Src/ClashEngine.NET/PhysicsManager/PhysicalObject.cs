using System;
using OpenTK;

namespace ClashEngine.NET.PhysicsManager
{
	using EntitiesManager;
	using Interfaces.EntitiesManager;
	using Interfaces.PhysicsManager;

	/// <summary>
	/// Komponent fizyczny.
	/// Po dodaniu do encji staje się ona zależna od wszystkich czynników zewnętrznych.
	/// </summary>
	public class PhysicalObject
		: Component, IPhysicalObject
	{
		#region Private fields
		private IAttribute<IVelocitiesCollection> Velocities_ = null;
		private IAttribute<Vector2> Position_ = null;

		/// <summary>
		/// Suma lokalnych(per-obiekt) prędkości. Ułatwia obliczenia.
		/// </summary>
		internal Vector2 LocalVelocity = new Vector2(0.0f, 0.0f);
		#endregion

		#region IPhysics Members
		/// <summary>
		/// Lista z prędkościami od których jest zależny dany obiekt.
		/// </summary>
		public IVelocitiesCollection Velocities
		{
			get { return this.Velocities_.Value; }
		}

		/// <summary>
		/// Pozycja obiektu.
		/// </summary>
		public Vector2 Position
		{
			get { return this.Position_.Value; }
			set { this.Position_.Value = value; }
		}

		public void Add(IVelocity velocity)
		{
			throw new NotImplementedException();
		}

		public void Remove(IVelocity velocity)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Component Members
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			this.Velocities_ = this.Owner.GetOrCreateAttribute<IVelocitiesCollection>("Velocities");
			this.Velocities_.Value = new InternalVelocitiesCollection(this); //Tego nie mogło być, więc musimy utworzyć nowe.

			this.Position_ = this.Owner.GetOrCreateAttribute<Vector2>("Position");
		}

		public override void Update(double delta)
		{
			this.Position += (float)delta * this.LocalVelocity;
		}
		#endregion

		#region IEquatable<IComponent> Members
		public bool Equals(IComponent other)
		{
			return (other is PhysicalObject) && other == this;
		}
		#endregion

		public PhysicalObject()
			: base("PhysicalObject")
		{ }
	}
}
