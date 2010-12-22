namespace ClashEngine.NET
{
	using Interfaces;
	using Extensions;

	/// <summary>
	/// Manager fizyki.
	/// Zaimplementowany jako singleton.
	/// Domyślnie przyjmuje grawitacje
	/// </summary>
	public class PhysicsManager
		: IPhysicsManager
	{
		#region Private fields
		private const float DefaultPhysicsTimeStep = 1f / 60f;
		private float Accumulator = 0f;
		#endregion

		#region Singleton
		private static PhysicsManager _Instance = null;

		/// <summary>
		/// Instancja managera fizyki.
		/// </summary>
		public static IPhysicsManager Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new PhysicsManager();
				}
				return _Instance;
			}
		}
		#endregion

		#region IPhysicsManager Members
		/// <summary>
		/// Świat.
		/// </summary>
		public FarseerPhysics.Dynamics.World World { get; private set; }

		/// <summary>
		/// Grawitacja.
		/// Odpowiada World.Gravity.
		/// </summary>
		public OpenTK.Vector2 Gravity
		{
			get
			{
				return this.World.Gravity.ToOpenTK();
			}
			set
			{
				this.World.Gravity = value.ToXNA();
			}
		}

		/// <summary>
		/// Krok czasowy fizyki.
		/// </summary>
		public float TimeStep { get; set; }

		/// <summary>
		/// Uaktualnia World.
		/// </summary>
		public void Update(double delta)
		{
			this.Accumulator += (float)delta;
			while (this.Accumulator >= this.TimeStep)
			{
				this.World.Step(this.TimeStep);
				this.Accumulator -= this.TimeStep;
			}
		}
		#endregion

		private PhysicsManager()
		{
			this.World = new FarseerPhysics.Dynamics.World(new Microsoft.Xna.Framework.Vector2(0.0f, 10.0f));
			this.TimeStep = DefaultPhysicsTimeStep;
		}
	}
}
