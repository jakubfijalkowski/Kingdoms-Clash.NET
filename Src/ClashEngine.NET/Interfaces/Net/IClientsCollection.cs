using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Lista klientów.
	/// Użytkownik nie może ręcznie dodawać klientów za to może ich usuwać(co wymusza zamknięcie połączenia).
	/// </summary>
	public interface IClientsCollection
		: IList<IClient>
	{ }
}
