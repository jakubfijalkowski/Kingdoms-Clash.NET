namespace ClashEngine.NET.PhysicsManager
{
	using Interfaces.PhysicsManager;
	using Utilities;

	/// <summary>
	/// Manager fizyki.
	/// Zaimplementowany jako singleton.
	/// Domyślnie przyjmuje grawitacje
	/// </summary>
	public class PhysicsManager
		: IPhysicsManager
	{
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
		#endregion

		private PhysicsManager()
		{
			this.World = new FarseerPhysics.Dynamics.World(new Microsoft.Xna.Framework.Vector2(0.0f, 10.0f));
		}
	}
}
