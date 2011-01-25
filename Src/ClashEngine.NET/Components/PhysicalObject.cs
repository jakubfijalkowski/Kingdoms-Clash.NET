using FarseerPhysics.Dynamics;

namespace ClashEngine.NET.Components
{
	using EntitiesManager;
	using Interfaces.Components;
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Komponent zmieniający encję w encję fizyczną(jest uzależniona od fizyki i managera fizyki).
	/// Zamienia atrybut Position tak by był synchronizowany z ciałem.
	/// </summary>
	/// <seealso cref="IPhysicsManager"/>
	public class PhysicalObject
		: Component, IPhysicalObject
	{
		#region Private fields
		private bool IsDynamic = false;
		private IAttribute<Body> _Body = null;
		#endregion

		#region IPhysicalObject Members
		/// <summary>
		/// Ciało obiektu.
		/// </summary>
		public Body Body
		{
			get { return this._Body.Value; }
			private set { this._Body.Value = value; }
		}
		#endregion

		#region Component Members
		/// <summary>
		/// Tworzymy ciało, dodajemy do managera i zamieniamy atrybut Position na taki, który trzyma pozycje zgodną z silnikiem fizycznym.
		/// </summary>
		public override void OnInit()
		{
			this._Body = this.Owner.Attributes.GetOrCreate<Body>("Body");
			this.Body = FarseerPhysics.Factories.BodyFactory.CreateBody(PhysicsManager.Instance.World);
			this.Body.BodyType = (this.IsDynamic ? BodyType.Dynamic : BodyType.Static);

			this.Owner.Attributes.Replace("Position", new Internals.PhysicalPositionAttribute("Position", this.Body));
		}

		/// <summary>
		/// Usuwamy ze świata.
		/// </summary>
		public override void OnDeinit()
		{
			PhysicsManager.Instance.World.RemoveBody(this.Body);
		}

		/// <summary>
		/// Niepotrzebujemy.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{ }
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
	}
}
