using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private List<IPlayerData> ReadyToPlayPlayers = new List<IPlayerData>();
		private TimeSpan TimeLeft;
		private TimeSpan TimeLeft2;

		private MultiplayerGameState GameState = null;
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

			this.TimeLeft = (this.TimeLeft2 = ServerConfiguration.Instance.WaitTime) + Settings.WaitTimeDelay;

			Stopwatch watch = new Stopwatch();
			double old = watch.Elapsed.TotalSeconds;
			while (!this.StopMainLoop)
			{
				double delta = watch.Elapsed.TotalSeconds - old;
				old = watch.Elapsed.TotalSeconds;

				this.ProcessClients(delta);
				this.ProcessOther(delta);
				if (this.InGame)
					this.ProcessInGame(delta);
				else
					this.ProcessNonInGame(delta);
			}
			watch.Stop();
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
		private void ProcessClients(double delta)
		{
			this.Server.Clients.RWLock.EnterUpgradeableReadLock();
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

					var accepted = new Messages.PlayerAccepted(userData.UserId, false, ServerConfiguration.Instance.GameController, ServerConfiguration.Instance.VictoryRules);
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
			this.Server.Clients.RWLock.ExitUpgradeableReadLock();
		}

		/// <summary>
		/// Obsługa właściwej gry.
		/// </summary>
		private void ProcessInGame(double delta)
		{

		}

		/// <summary>
		/// Obsługa serwera, gdy nie jesteśmy w grze.
		/// </summary>
		private void ProcessNonInGame(double delta)
		{
			if (this.ReadyToPlayPlayers.Count >= 2)
			{
				this.TimeLeft2.Subtract(new TimeSpan((long)(delta * TimeSpan.TicksPerSecond)));
				if (this.TimeLeft2.TotalSeconds == 0.0)
				{
					this.StartGame();
				}
				else if (this.TimeLeft - this.TimeLeft2 > Settings.WaitTimeDelay)
				{
					var secondsLeft = (int)Math.Floor(this.TimeLeft2.TotalSeconds);
					this.Server.Clients.SendToAll(new Messages.GameWillStartAfter(new TimeSpan(0, 0, secondsLeft)).ToMessage());
					this.TimeLeft = this.TimeLeft2;
				}
			}
			else
			{
				this.TimeLeft = ServerConfiguration.Instance.WaitTime;
			}
		}

		/// <summary>
		/// Obsługa innych rzeczy, niezwiązanych z grą.
		/// </summary>
		private void ProcessOther(double delta)
		{
			foreach (var client in this.Server.Clients)
			{
				client.Messages.RWLock.EnterUpgradeableReadLock();
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
								this.ReorderPlayersList(client.UserData as IPlayerData);
							}
							break;

						default:
							client.Messages.RWLock.ExitUpgradeableReadLock();
							continue;
					}
					client.Messages.RWLock.EnterWriteLock();
					client.Messages.RemoveAt(0);
					client.Messages.RWLock.ExitWriteLock();
				}
				client.Messages.RWLock.ExitUpgradeableReadLock();
			}
		}
		#endregion

		#region Other
		private void ReorderPlayersList(IPlayerData player)
		{
			if (player.ReadyToPlay)
			{
				this.ReadyToPlayPlayers.Add(player);
			}
			else
			{
				this.ReadyToPlayPlayers.Remove(player);
			}
		}

		private void StartGame()
		{
			this.GameState = new MultiplayerGameState(this.GameInfo);
			//this.Server.Clients.SendToAll(new Messages.GameStarted());
		}
		#endregion
	}
}
