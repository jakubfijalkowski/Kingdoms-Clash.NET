using System.Net;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Klasa serwera.
	/// </summary>
	public interface IServer
	{
		/// <summary>
		/// Dane, na których serwer nasłuchuje.
		/// </summary>
		IPEndPoint Endpoint { get; }

		/// <summary>
		/// Maksymalna liczba klientów podłączonych do serwera.
		/// </summary>
		uint MaxClients { get; }

		/// <summary>
		/// Lista klientów aktualnie podłączonych do serwera.
		/// </summary>
		IClientsCollection Clients { get; }
	}
}
