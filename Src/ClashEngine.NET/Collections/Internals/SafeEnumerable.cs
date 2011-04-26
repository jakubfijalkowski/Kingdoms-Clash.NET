using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ClashEngine.NET.Collections.Internals
{
	internal class SafeEnumerable<T>
		: IEnumerable<T>
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IEnumerable<T> Collection;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ReaderWriterLockSlim RWLock;
		#endregion

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return new SafeEnumerator<T>(this.Collection, this.RWLock);
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SafeEnumerator<T>(this.Collection, this.RWLock);
		}
		#endregion

		#region Constructors
		public SafeEnumerable(IEnumerable<T> collection, ReaderWriterLockSlim rwLock)
		{
			this.Collection = collection;
			this.RWLock = rwLock;
		}
		#endregion
	}
}
