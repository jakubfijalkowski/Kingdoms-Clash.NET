using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ClashEngine.NET.Collections
{
	using Interfaces.Collections;

	/// <summary>
	/// Thread-safe enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SafeEnumerator<T>
		: ISafeEnumerator<T>
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IEnumerator<T> _Original;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ReaderWriterLockSlim _RWLock;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly bool _Upgradeable;
		#endregion

		#region ISafeEnumerator<T> Members
		/// <summary>
		/// Bazowy enumerator.
		/// </summary>
		public IEnumerator<T> Original
		{
			get { return this._Original; }
		}

		/// <summary>
		/// Lock.
		/// </summary>
		public ReaderWriterLockSlim RWLock
		{
			get { return this._RWLock; }
		}

		/// <summary>
		/// Czy enumerator jest w trybie "upgradeable".
		/// </summary>
		public bool Upgradeable
		{
			get { return this._Upgradeable; }
		}
		#endregion

		#region IEnumerator<T> Members
		public T Current
		{
			get { return this.Original.Current; }
		}
		#endregion

		#region IEnumerator Members
		object IEnumerator.Current
		{
			get { return this.Current; }
		}

		public bool MoveNext()
		{
			return this.Original.MoveNext();
		}

		public void Reset()
		{
			this.Original.Reset();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje enumerator.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="rwLock"></param>
		public SafeEnumerator(IEnumerable<T> collection, ReaderWriterLockSlim rwLock)
			: this(collection, rwLock, false)
		{ }

		/// <summary>
		/// Inicjalizuje enumerator.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="rwLock"></param>
		/// <param name="upgradeable">Czy wejść w tryb upgradeable, czy w tryb read.</param>
		public SafeEnumerator(IEnumerable<T> collection, ReaderWriterLockSlim rwLock, bool upgradeable)
		{
			this._RWLock = rwLock;
			if (this._Upgradeable = upgradeable)
				this._RWLock.EnterUpgradeableReadLock();
			else
				this._RWLock.EnterReadLock();
			this._Original = collection.GetEnumerator();
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this._Original.Dispose();
			if (this._Upgradeable)
				this._RWLock.ExitUpgradeableReadLock();
			else
				this._RWLock.ExitReadLock();
		}
		#endregion
	}

	public static class IEnumerableExt
	{
		/// <summary>
		/// Zwraca kolekcję do enumeracji jako thread-safe.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="original">Oryginalna kolekcja.</param>
		/// <param name="rwLock">Lock, którego mamy użyć.</param>
		/// <param name="upgradeable">Czy kolekcja ma być w trybie "upgradeable", czy w trybie "read".</param>
		/// <returns></returns>
		public static IEnumerable<T> AsLocked<T>(this IEnumerable<T> original, ReaderWriterLockSlim rwLock, bool upgradeable)
		{
			if (upgradeable)
				return new Internals.UpgradeableEnumerable<T>(original, rwLock);
			else
				return new Internals.SafeEnumerable<T>(original, rwLock);
		}
	}
}
