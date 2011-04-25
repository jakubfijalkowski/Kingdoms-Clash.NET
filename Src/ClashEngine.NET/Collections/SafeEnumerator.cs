using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ClashEngine.NET.Collections
{
	/// <summary>
	/// Thread-safe enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SafeEnumerator<T>
		: IEnumerator<T>
	{
		#region Private fields
		private readonly IEnumerator<T> Original;
		private readonly object SyncRoot;
		#endregion

		#region IEnumerator<T> Members
		public T Current
		{
			get { return Original.Current; }
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
		/// <param name="original"></param>
		/// <param name="syncRoot"></param>
		public SafeEnumerator(IEnumerator<T> original, object syncRoot)
		{
			this.Original = original;
			this.SyncRoot = syncRoot;
			Monitor.Enter(this.SyncRoot);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Monitor.Exit(this.SyncRoot);
		}
		#endregion
	}
}
