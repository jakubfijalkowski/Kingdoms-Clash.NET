using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ClashEngine.NET.Collections
{
	using Interfaces.Collections;

	/// <summary>
	/// Kolekcja thread-safe.
	/// </summary>
	/// <remarks>
	/// Bazuje na List&lt;T&gt;.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	[DebuggerDisplay("Count = {Count}")]
	public class SafeCollection<T>
		: ISafeCollection<T>
	{
		#region Private and protected fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ReaderWriterLockSlim _RWLock = new ReaderWriterLockSlim();
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		protected readonly List<T> InnerList = new List<T>();
		#endregion

		#region ISafeEnumerable<T> Members
		/// <summary>
		/// Obiekt do synchronizacji.
		/// </summary>
		public ReaderWriterLockSlim RWLock
		{
			get { return this._RWLock; }
		}

		/// <summary>
		/// Pobiera enumerator, który jest w trybie "upgradeable".
		/// </summary>
		/// <returns></returns>
		public ISafeEnumerator<T> GetUpgradeableEnumerator()
		{
			return new SafeEnumerator<T>(this.GetEnumerator(), this._RWLock, true);
		}

		/// <summary>
		/// Pobiera enumerator, który jest w trybie "upgradeable".
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> GetUpgradeableEnumerable()
		{
			return new Internals.UpgradeableEnumerable<T>(this.GetEnumerator(), this._RWLock);
		}
		#endregion

		#region ICollection<T> Members
		/// <summary>
		/// Dodaje element do kolekcji.
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			this._RWLock.EnterWriteLock();
			this.InnerList.Add(item);
			this._RWLock.ExitWriteLock();
		}

		/// <summary>
		/// Usuwa element z kolekcji.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			this._RWLock.EnterWriteLock();
			var r = this.InnerList.Remove(item);
			this._RWLock.ExitWriteLock();
			return r;
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this._RWLock.EnterWriteLock();
			this.InnerList.Clear();
			this._RWLock.ExitWriteLock();
		}
		
		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wskazany element.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			this._RWLock.EnterReadLock();
			var r = this.InnerList.Contains(item);
			this._RWLock.EnterReadLock();
			return r;
		}

		/// <summary>
		/// Kopiuje kolekcję do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			this._RWLock.EnterWriteLock();
			this.InnerList.CopyTo(array, arrayIndex);
			this._RWLock.ExitWriteLock();
		}

		/// <summary>
		/// Pobiera liczbę elementów w kolekcji.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count
		{
			get { return this.InnerList.Count; }
		}

		/// <summary>
		/// Nieużywane.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return new SafeEnumerator<T>(this.GetEnumerator(), this._RWLock);
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new SafeEnumerator<T>(this.GetEnumerator(), this._RWLock);
		}
		#endregion
	}
}
