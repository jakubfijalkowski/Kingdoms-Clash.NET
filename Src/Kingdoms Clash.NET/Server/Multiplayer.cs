using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;
	using NET.Interfaces;
	using NET.Interfaces.Player;

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
				this.ProcessClients();
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
		/// Obsługa nowych i rozłączonych klientów.
		/// </summary>
		private void ProcessClients()
		{
			for (int i = 0; i < this.Server.Clients.Count; i++)
			{
				var client = this.Server.Clients[i];
				if (client.Status != ClientStatus.Ok && client.Status != ClientStatus.Welcome)
				{
					this.Server.Clients.RemoveAt(i--);
					if (client.UserData != null) //Zabezpiecza przed ewentualnym rozłączeniem się użytkownika przed
					{
						if (this.InGame && (client.UserData as Player.PlayerData).InGame)
						{
							//TODO: zakończenie gry
						}

						this.Server.Clients.SendToAll(new Messages.PlayerDisconnected(
							(client.UserData as IPlayerData).UserId, (DisconnectionReason)client.Status
							).ToMessage());
					}
				}
				else if (client.UserData == null && client.Messages.Contains((MessageType)GameMessageType.PlayersFirstConfiguration))
				{
					//Rozpakowanie pierwszej wiadomości
					var msg = new Messages.PlayersFirstConfiguration(client.Messages.GetFirst((MessageType)GameMessageType.PlayersFirstConfiguration));
					var userData = new Player.PlayerData(this.NextUserId++);
					userData.InGame = false;
					userData.Nick = msg.Nick;

					var accepted = new Messages.PlayerAccepted(userData.UserId, false);
					for (int j = 0; j < this.Server.Clients.Count; j++)
					{
						if (this.Server.Clients[j].UserData != null)
						{
							accepted.Players.Add(this.Server.Clients[j].UserData as IPlayerData);
						}
					}
					client.Send(accepted.ToMessage());
					this.Server.Clients.SendToAll(new Messages.PlayerConnected(userData.UserId, userData.Nick).ToMessage(),
						c => c != client);
					client.UserData = userData;
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
			for (int i = 0; i < this.Server.Clients.Count; i++)
			{
				var client = this.Server.Clients[i];
				if (client.UserData != null && client.Messages.Count > 0)
				{
					switch ((GameMessageType)client.Messages[0].Type)
					{
						case GameMessageType.PlayerChangedNick: //Zmiana nicku
							{
								var msg = new Messages.PlayerChangedNick(client.Messages[0]);
								msg.UserId = (client.UserData as IPlayerData).UserId;
								(client.UserData as IPlayerData).Nick = msg.NewNick;
								this.Server.Clients.SendToAll(msg.ToMessage(), c => c != client);
							}
							break;

						case GameMessageType.PlayerChangedState:
							{
								(client.UserData as IPlayerData).ReadyToPlay = !(client.UserData as IPlayerData).ReadyToPlay;
								var msg = new Messages.PlayerChangedState((client.UserData as IPlayerData).UserId);
								this.Server.Clients.SendToAll(msg.ToMessage(), c => c != client);
							}
							break;

						default:
							continue;
					}
					client.Messages.RemoveAt(0);
				}
			}
		}
		#endregion
	}
}
