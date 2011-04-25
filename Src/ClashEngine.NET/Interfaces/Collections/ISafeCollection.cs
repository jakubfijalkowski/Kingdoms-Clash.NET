using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Collections
{
	/// <summary>
	/// Kolekcja thread-safe.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISafeCollection<T>
		: ICollection<T>
	{
		/// <summary>
		/// Obiekt do synchronizacji.
		/// </summary>
		object SyncRoot { get; }
	}
}
