using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;

	/// <summary>
	/// Obsługuje grę multiplayer - połączenie z klientami, czas oczekiwania na rozpoczęcie, itp.
	/// </summary>
	public class Multiplayer
		: IMultiplayer
	{
		#region Private fields
		private bool InGame = false;
		private bool StopMainLoop = false;
		private IServer Server = null;
		private uint NextUserId = 0;
		#endregion

		#region IMultiplayer Members
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		public IGameInfo GameInfo { get; private set; }

		/// <summary>
		/// Uruchamia grę.
		/// </summary>
		public void Start()
		{
			this.Server = new TcpServer(ServerConfiguration.Instance.Port, ServerConfiguration.Instance.MaxSpectators + 2,
				ServerConfiguration.Instance.Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version,
				ServerConfiguration.Instance.InfoPort);

			this.Server.Start();
			while (!this.StopMainLoop)
			{
				this.ProcessUsers();
				this.ProcessOther();
				if (this.InGame)
					this.ProcessInGame();
				else
					this.ProcessNonInGame();
			}
			this.Server.Stop();
		}

		/// <summary>
		/// Kończy grę.
		/// </summary>
		public void Stop()
		{
			this.StopMainLoop = true;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje.
		/// </summary>
		/// <param name="info"></param>
		public Multiplayer(IGameInfo info)
		{
			this.GameInfo = info;
		}
		#endregion

		#region Main methods
		/// <summary>
		/// Obsługa nowych użytkowników oraz tych, którzy się odłączyli.
		/// </summary>
		private void ProcessUsers()
		{
			foreach (var client in this.Server.Clients)
			{
				if (client.Status != ClientStatus.Ok)
				{
					this.PlayerDisconnected(client);
				}
				else if (client.UserData == null)
				{
					client.UserData = new Player.PlayerData(this.NextUserId++); //Dodanie nowego użytkownika
				}
			}
		}

		/// <summary>
		/// Obsługa właściwej gry.
		/// </summary>
		private void ProcessInGame()
		{

		}

		/// <summary>
		/// Obsługa serwera, gdy nie jesteśmy w grze.
		/// </summary>
		private void ProcessNonInGame()
		{

		}

		/// <summary>
		/// Obsługa innych rzeczy, niezwiązanych z grą.
		/// </summary>
		private void ProcessOther()
		{

		}
		#endregion

		#region Behaviors
		private void PlayerDisconnected(IClient client)
		{
			if (this.InGame && (client.UserData as Player.PlayerData).Player != null)
			{

			}
			this.Server.Clients.Remove(client);
			//this.Server.Clients.SendToAll(new ClientDisconnected(client));
		}
		#endregion
	}
}
