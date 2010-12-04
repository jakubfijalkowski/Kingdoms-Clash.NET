using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units.XAML
{
	using Interfaces.Resources;

	/// <summary>
	/// Kolekcja zasobów - używana do ładniejszej integracji z XAML-em.
	/// </summary>
	public class ResourcesCollection
		: ICollection<IResource>
	{
		private IResourcesCollection Destination = null;

		#region ICollection<IResource> Members
		public void Add(IResource item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Destination.Add(item.Name, item.Value);
		}

		public bool Remove(IResource item)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(IResource item)
		{
			throw new NotSupportedException();
		}

		public void CopyTo(IResource[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get { return this.Destination.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IResource> Members
		public IEnumerator<IResource> GetEnumerator()
		{
			throw new NotSupportedException();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}
		#endregion

		internal ResourcesCollection(IResourcesCollection coll)
		{
			this.Destination = coll;
		}
	}
}
