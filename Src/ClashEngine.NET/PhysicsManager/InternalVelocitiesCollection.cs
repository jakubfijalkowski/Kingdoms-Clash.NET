using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.PhysicsManager
{
	using Interfaces.PhysicsManager;

	/// <summary>
	/// Lista z prędkościami która aktualizuje LocalVelocity komponentu.
	/// </summary>
	internal class InternalVelocitiesCollection
		: IVelocitiesCollection
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		private List<IVelocity> InternalList = new List<IVelocity>();
		private PhysicalObject Parent;

		#region ICollection<IVelocity> Members
		public void Add(IVelocity item)
		{
			if (this.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			Logger.Debug("Velocity '{0}' added to {1}", item.Name, this.Parent.Id);
			this.InternalList.Add(item);
			this.Parent.LocalVelocity += item.Value;
		}

		public void Clear()
		{
			Logger.Debug("Velocities in object {0} cleared", this.Parent.Id);
			this.InternalList.Clear();
			this.Parent.LocalVelocity = Vector2.Zero;
		}

		public bool Contains(IVelocity item)
		{
			return this.InternalList.Contains(item);
		}

		public void CopyTo(IVelocity[] array, int arrayIndex)
		{
			this.InternalList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this.InternalList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(IVelocity item)
		{
			int i = this.InternalList.IndexOf(item);
			if (i > -1)
			{
				Logger.Debug("Velocity '{0}' removed from {1}", item.Name, this.Parent.Id);
				this.Parent.LocalVelocity -= this.InternalList[i].Value;
				this.InternalList.RemoveAt(i);
				return true;
			}
			return false;
		}
		#endregion

		#region IEnumerable<IVelocity> Members
		public IEnumerator<IVelocity> GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}
		#endregion

		public InternalVelocitiesCollection(PhysicalObject parent)
		{
			this.Parent = parent;
		}
	}
}
