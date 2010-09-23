using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace ClashEngine.NET.PhysicsManager
{
	using Interfaces.PhysicsManager;
	using Interfaces.Components;

	/// <summary>
	/// Manager fizyki.
	/// </summary>
	public class PhysicsManager
		: IPhysicsManager
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
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
		/// Teren.
		/// </summary>
		public ITerrain Terrain { get; set; }

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
		{
			new public void Add(IVelocity item)
			{
				if (this.Contains(item))
				{
					throw new Exceptions.ArgumentAlreadyExistsException("item");
				}
				Logger.Debug("Global velocity '{0}' added", item.Name);
				base.Add(item);
			}

			new public void Clear()
			{
				Logger.Debug("Global velocities list cleared");
				base.Clear();
			}

			new public bool Remove(IVelocity item)
			{
				int i = base.IndexOf(item);
				if (i > -1)
				{
					Logger.Debug("Global velocity '{0}' removed", item.Name);
					base.RemoveAt(i);
					return true;
				}
				return false;
			}
		}
	}
}
