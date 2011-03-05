using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Lista klientów.
	/// Zawsze read-only.
	/// </summary>
	public interface IClientsCollection
		: IList<IClient>
	{ }
}
