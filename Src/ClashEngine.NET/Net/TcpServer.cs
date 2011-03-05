using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Serwer oparty na połączeniach TCP.
	/// </summary>
	public class TcpServer
		: IServer
	{
		#region Private fields
		private Internals.ServerClientsCollection _ClientsCollection = new Internals.ServerClientsCollection();
		#endregion

		#region IServer Members
		/// <summary>
		/// Dane, na których serwer nasłuchuje.
		/// </summary>
		public IPEndPoint Endpoint { get; private set; }

		/// <summary>
		/// Maksymalna liczba klientów podłączonych do serwera.
		/// </summary>
		public uint MaxClients { get; private set; }

		/// <summary>
		/// Lista klientów aktualnie podłączonych do serwera.
		/// </summary>
		public IClientsCollection Clients
		{
			get { return this._ClientsCollection; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje serwer.
		/// </summary>
		/// <param name="endpoint"><see cref="Endpoint"/></param>
		/// <param name="maxClients"><see cref="MaxClients"/></param>
		public TcpServer(IPEndPoint endpoint, uint maxClients)
		{
			this.Endpoint = endpoint;
			this.MaxClients = maxClients;
		}

		/// <summary>
		/// Inicjalizuje serwer.
		/// </summary>
		/// <param name="port">Port, na którym serwer nasłuchuje.</param>
		/// <param name="maxClients"><see cref="MaxClients"/></param>
		public TcpServer(int port, uint maxClients)
			: this(new IPEndPoint(IPAddress.Any, port), maxClients)
		{ }
		#endregion
	}
}
