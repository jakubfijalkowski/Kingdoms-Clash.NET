using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace ClashEngine.NET.Collections
{
	using Interfaces.Collections;

	/// <summary>
	/// Kolekcja thread-safe.
	/// </summary>
	/// <remarks>
	/// Bazuje na List&lt;T&gt;.
	/// </remarks>
	[DebuggerDisplay("Count = {Count}")]
	public class SafeCollection<T>
		: ISafeCollection<T>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly object _SyncRoot = new object();
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		protected List<T> InnerList = new List<T>();

		#region ISafeCollection<T> Members
		/// <summary>
		/// Obiekt do synchronizacji.
		/// </summary>
		public object SyncRoot
		{
			get { return this._SyncRoot; }
		}
		#endregion

		#region ICollection<T> Members
		public void Add(T item)
		{
			lock (this._SyncRoot)
			{
				this.InnerList.Add(item);
			}
		}

		public bool Remove(T item)
		{
			lock (this._SyncRoot)
			{
				return this.Remove(item);
			}
		}

		public void Clear()
		{
			lock (this._SyncRoot)
			{
				this.InnerList.Clear();
			}
		}

		public bool Contains(T item)
		{
			lock (this._SyncRoot)
			{
				return this.InnerList.Contains(item);
			}
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			lock (this._SyncRoot)
			{
				this.InnerList.CopyTo(array, arrayIndex);
			}
		}

		public int Count
		{
			get { return this.InnerList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
