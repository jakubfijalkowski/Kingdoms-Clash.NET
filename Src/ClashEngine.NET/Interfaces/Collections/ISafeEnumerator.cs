using System.Collections.Generic;
using System.Threading;

namespace ClashEngine.NET.Interfaces.Collections
{
	/// <summary>
	/// Thread-safe enumerator.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISafeEnumerator<T>
		: IEnumerator<T>
	{
		/// <summary>
		/// Bazowy enumerator.
		/// </summary>
		IEnumerator<T> Original { get; }

		/// <summary>
		/// Lock.
		/// </summary>
		ReaderWriterLockSlim RWLock { get; }

		/// <summary>
		/// Czy enumerator jest w trybie "upgradeable".
		/// </summary>
		bool Upgradeable { get; }
	}
}
