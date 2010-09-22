using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace ClashEngine.NET.PhysicsManager
{
	using Interfaces.PhysicsManager;

	/// <summary>
	/// Manager fizyki.
	/// </summary>
	public class PhysicsManager
		: IPhysicsManager
	{
		#region Singleton
		private static PhysicsManager Instance_ = null;

		/// <summary>
		/// Instancja managera.
		/// </summary>
		public static IPhysicsManager Instance
		{
			get
			{
				if (Instance_ == null)
				{
					Instance_ = new PhysicsManager();
				}
				return Instance_;
			}
		}
		#endregion

		#region IPhysicsManager Members
		/// <summary>
		/// Prędkości.
		/// </summary>
		public IVelocitiesCollection Velocities { get; private set; }

		/// <summary>
		/// Sumuje wektory prędkości.
		/// Pozwala na wyłączenie wskazanych prędkości.
		/// </summary>
		public Vector2 CalculateVelocities(params string[] exclude)
		{
			if (exclude == null)
			{
				exclude = new string[0];
			}
			Vector2 velocity = Vector2.Zero;
			foreach (var v in this.Velocities.Where(v => !exclude.Contains(v.Name)))
			{
				velocity += v.Value;
			}
			return velocity;
		}
		#endregion

		private PhysicsManager()
		{
			this.Velocities = new VelocitiesCollection();
		}

		/// <summary>
		/// Lista prędkości bez przypisania do danego obiektu.
		/// </summary>
		private class VelocitiesCollection
			: List<IVelocity>, IVelocitiesCollection
		{ }
	}
}
