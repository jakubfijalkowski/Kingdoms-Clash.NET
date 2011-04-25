namespace ClashEngine.NET.Collections
{
	using Interfaces.Collections;

	/// <summary>
	/// Lista thread-safe.
	/// </summary>
	/// <remarks>
	/// Bazuje na SafeCollection.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class SafeList<T>
		: SafeCollection<T>, ISafeList<T>
	{
		#region IList<T> Members
		/// <summary>
		/// Pobiera indeks wskazanego elementu.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			base.RWLock.EnterReadLock();
			var i = base.InnerList.IndexOf(item);
			base.RWLock.ExitReadLock();
			return i;
		}

		/// <summary>
		/// Wstawia element na wskazane miejsce.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, T item)
		{
			try
			{
				base.RWLock.EnterWriteLock();
				base.InnerList.Insert(index, item);
			}
			finally
			{
				base.RWLock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Usuwa element ze wskazanego miejsca.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			try
			{
				base.RWLock.EnterWriteLock();
				base.InnerList.RemoveAt(index);
			}
			finally
			{
				base.RWLock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Pobiera lub zmienia element ze wskazanego miejsca.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				try
				{
					base.RWLock.EnterReadLock();
					return base.InnerList[index];
				}
				finally
				{
					base.RWLock.ExitReadLock();
				}
			}
			set
			{
				try
				{
					base.RWLock.EnterWriteLock();
					base.InnerList[index] = value;
				}
				finally
				{
					base.RWLock.ExitWriteLock();
				}
			}
		}
		#endregion
	}
}
