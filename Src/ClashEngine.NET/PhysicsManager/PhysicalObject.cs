using FarseerPhysics.Dynamics;

namespace ClashEngine.NET.PhysicsManager
{
	using EntitiesManager;
	using Interfaces.EntitiesManager;
	using Interfaces.PhysicsManager;

	/// <summary>
	/// Komponent zmieniający encję w encję fizyczną(jest uzależniona od fizyki i managera fizyki).
	/// Zamienia atrybut Position tak by był synchronizowany z ciałem.
	/// </summary>
	/// <seealso cref="IPhysicsManager"/>
	public class PhysicalObject
		: Component, IPhysicalObject
	{
		private IAttribute<Body> Body_ = null;

		#region IPhysicalObject Members
		/// <summary>
		/// Ciało obiektu.
		/// </summary>
		public Body Body
		{
			get { return this.Body_.Value; }
			private set { this.Body_.Value = value; }
		}
		#endregion

		public PhysicalObject()
			: base("PhysicalObject")
		{ }

		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			this.Body_ = this.Owner.Attributes.GetOrCreate<Body>("Body");
			this.Body = PhysicsManager.Instance.World.CreateBody();

			this.Owner.Attributes.Replace("Position", new PhysicalPositionAttribute("Position", this.Body));
		}

		public override void Update(double delta)
		{ }
	}
}
