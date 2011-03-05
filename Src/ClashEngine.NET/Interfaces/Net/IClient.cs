using System.Net;

namespace ClashEngine.NET.Interfaces.Net
{
	/// <summary>
	/// Klient.
	/// </summary>
	public interface IClient
	{
		/// <summary>
		/// Informacje o kliencie.
		/// </summary>
		IPEndPoint Endpoint { get; }

		/// <summary>
		/// Zamyka połączenie z klientem.
		/// </summary>
		void Close();
	}
}
