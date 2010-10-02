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
		private bool IsDynamic = false;
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

		/// <summary>
		/// Inicjalizuje komponent.
		/// </summary>
		/// <param name="isDynamic">True, jeśli obiekt ma być obiektem dynamicznym(odsyłam do dokumentacji FarseerPhysics lub Box2D).</param>
		public PhysicalObject(bool isDynamic = false)
			: base("PhysicalObject")
		{
			this.IsDynamic = isDynamic;
		}

		public override void OnInit()
		{
			this.Body_ = this.Owner.Attributes.GetOrCreate<Body>("Body");
			this.Body = PhysicsManager.Instance.World.CreateBody();
			this.Body.BodyType = (this.IsDynamic ? BodyType.Dynamic : BodyType.Static);

			this.Owner.Attributes.Replace("Position", new PhysicalPositionAttribute("Position", this.Body));
		}

		public override void Update(double delta)
		{ }
	}
}
