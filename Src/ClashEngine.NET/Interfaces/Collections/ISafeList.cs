using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Collections
{
	/// <summary>
	/// Lista thread-safe.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISafeList<T>
		: ISafeCollection<T>, IList<T>
	{ }
}
