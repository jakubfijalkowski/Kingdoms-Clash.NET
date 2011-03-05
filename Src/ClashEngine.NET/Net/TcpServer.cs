using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Serwer oparty na połączeniach TCP.
	/// </summary>
	public class TcpServer
		: IServer
	{
		#region Consts
		private const int ListenBacklog = 10;
		#endregion

		#region Private fields
		private Internals.ServerClientsCollection _ClientsCollection = new Internals.ServerClientsCollection();
		private Thread ServerThread = null;
		private bool ServerStop = false;
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

		/// <summary>
		/// Czy serwer jest uruchomiony.
		/// </summary>
		public bool IsRunning
		{
			get { return this.ServerThread.IsAlive; }
		}

		/// <summary>
		/// Nazwa gry.
		/// </summary>
		public string GameName { get; private set; }

		/// <summary>
		/// Wersja gry.
		/// </summary>
		public Version GameVersion { get; private set; }

		/// <summary>
		/// Startuje serwer na nowym wkątku.
		/// </summary>
		public void Start()
		{
			if (!this.IsRunning)
			{
				this.ServerThread.Start();
			}
		}

		/// <summary>
		/// Zatrzymuje działanie serwera rozłączając wszystkich klientów i kończąc dodatkowy wątek.
		/// </summary>
		public void Stop()
		{
			this.ServerStop = true;
			while (this.IsRunning) Thread.Sleep(100);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje serwer.
		/// </summary>
		/// <param name="endpoint"><see cref="Endpoint"/></param>
		/// <param name="maxClients"><see cref="MaxClients"/></param>
		/// <param name="gameName"><see cref="GameName"/></param>
		/// <param name="gameVersion"><see cref="GameVersion"/></param>
		public TcpServer(IPEndPoint endpoint, uint maxClients, string gameName, Version gameVersion)
		{
			this.Endpoint = endpoint;
			this.MaxClients = maxClients;
			this.GameName = gameName;
			this.GameVersion = gameVersion;

			this.ServerThread = new Thread(this.ServerMain);
		}

		/// <summary>
		/// Inicjalizuje serwer.
		/// </summary>
		/// <param name="port">Port, na którym serwer nasłuchuje.</param>
		/// <param name="maxClients"><see cref="MaxClients"/></param>
		public TcpServer(int port, uint maxClients, string gameName, Version gameVersion)
			: this(new IPEndPoint(IPAddress.Any, port), maxClients, gameName, gameVersion)
		{ }
		#endregion

		#region Private methods
		private void ServerMain()
		{
			Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listenSocket.Blocking = false;
			listenSocket.Bind(this.Endpoint);
			listenSocket.Listen(ListenBacklog);

			while (!this.ServerStop)
			{
				if (listenSocket.Poll(0, SelectMode.SelectRead)) //Mamy kogoś do dodania 
				{
					var client = new Internals.ServerClient(listenSocket.Accept());
					client.Send(new Messages.ServerWelcome(this.GameVersion, this.GameName).ToMessage());
					this._ClientsCollection.InternalAdd(client);
				}
			}
			this.ServerStop = false;
		}
		#endregion
	}
}
