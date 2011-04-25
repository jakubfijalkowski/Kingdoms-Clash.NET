using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ClashEngine.NET.Collections.Internals
{
	internal class UpgradeableEnumerable<T>
		: IEnumerable<T>
	{
		#region Private fields
		private readonly IEnumerator<T> Original;
		private readonly ReaderWriterLockSlim RWLock;
		#endregion

		#region IEnumerable<T> Members
		public IEnumerator<T> GetEnumerator()
		{
			return new SafeEnumerator<T>(this.Original, this.RWLock);
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SafeEnumerator<T>(this.Original, this.RWLock);
		}
		#endregion

		#region Constructors
		public UpgradeableEnumerable(IEnumerator<T> original, ReaderWriterLockSlim rwLock)
		{
			this.Original = original;
			this.RWLock = rwLock;
		}
		#endregion
	}
}
