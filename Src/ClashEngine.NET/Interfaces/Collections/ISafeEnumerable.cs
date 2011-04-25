using System.Collections.Generic;
using System.Threading;

namespace ClashEngine.NET.Interfaces.Collections
{
	/// <summary>
	/// Kolekcja wyliczeniowa
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISafeEnumerable<T>
		: IEnumerable<T>
	{
		/// <summary>
		/// Obiekt do synchronizacji.
		/// </summary>
		ReaderWriterLockSlim RWLock { get; }

		/// <summary>
		/// Pobiera enumerator, który jest w trybie "upgradeable".
		/// </summary>
		/// <returns></returns>
		ISafeEnumerator<T> GetUpgradeableEnumerator();

		/// <summary>
		/// Zwraca kolekcję, która zwraca IEnumerable skonfigurowany tak, by zwracał ISafeEnumerator z Upgradeable = true.
		/// Funkcja pomocna przy listowaniu elementów za pomocą enumeratora.
		/// </summary>
		/// <returns></returns>
		IEnumerable<T> GetUpgradeableEnumerable();
	}
}
