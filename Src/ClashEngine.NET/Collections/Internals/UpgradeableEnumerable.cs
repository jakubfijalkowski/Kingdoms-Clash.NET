using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ClashEngine.NET.Collections.Internals
{
	internal class UpgradeableEnumerable<T>
		: IEnumerable<T>
	{
		#region Private fields
		private readonly IEnumerable<T> Collection;
		private readonly ReaderWriterLockSlim RWLock;
		#endregion

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return new SafeEnumerator<T>(this.Collection, this.RWLock, true);
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SafeEnumerator<T>(this.Collection, this.RWLock, true);
		}
		#endregion

		#region Constructors
		public UpgradeableEnumerable(IEnumerable<T> collection, ReaderWriterLockSlim rwLock)
		{
			this.Collection = collection;
			this.RWLock = rwLock;
		}
		#endregion
	}
}
