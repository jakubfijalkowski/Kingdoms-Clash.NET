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
		private List<IVelocity> InternalList = new List<IVelocity>();
		private PhysicalObject Parent;

		#region ICollection<IVelocity> Members
		public void Add(IVelocity item)
		{
			if (this.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.InternalList.Add(item);
			this.Parent.LocalVelocity += item.Value;
		}

		public void Clear()
		{
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
