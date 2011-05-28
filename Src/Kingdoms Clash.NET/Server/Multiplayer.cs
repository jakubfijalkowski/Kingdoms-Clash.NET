using System;
using System.Collections.Generic;
using System.Diagnostics;
using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET.Server
{
	using NET;
	using Interfaces;
	using NET.Interfaces;
	using NET.Interfaces.Map;
	using NET.Interfaces.Player;
	using NET.Interfaces.Units;

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
		private List<IClient> ReadyToPlayPlayers = new List<IClient>();
		private TimeSpan TimeLeft;
		private TimeSpan TimeLeft2;

		private MultiplayerGameState GameState = null;

		private Dictionary<GameMessageType, Func<IClient, Message, bool>> Handlers = new Dictionary<GameMessageType, Func<IClient, Message, bool>>();
		private Dictionary<GameMessageType, Func<IClient, Message, bool>> InGameHandlers = new Dictionary<GameMessageType, Func<IClient, Message, bool>>();
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
					this.ProcessInGameMessages(delta);
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
			this.GameState = new MultiplayerGameState(this);

			this.Handlers.Add(GameMessageType.PlayerChangedNick, this.HandlePlayerChangedNick);
			this.Handlers.Add(GameMessageType.PlayerChangedState, this.HandlePlayerChangedState);

			this.InGameHandlers.Add(GameMessageType.UnitQueueAction, this.HandleUnitQueueAction);
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
		private void ProcessInGameMessages(double delta)
		{
			for (int i = 0; i < 2; i++)
			{
				foreach (var msg in this.ReadyToPlayPlayers[i].Messages.GetUpgradeableEnumerable())
				{
					if (this.InGameHandlers.Call(this.ReadyToPlayPlayers[i], msg))
					{
						this.ReadyToPlayPlayers[i].Messages.RemoveAt(0);
					}
					break;
				}
			}
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
				if (client.UserData != null)
				{
					foreach (var msg in client.Messages.GetUpgradeableEnumerable())
					{
						this.Handlers.Call(client, msg);
						client.Messages.RemoveAt(0);
						break;
					}
				}
			}
		}
		#endregion

		#region Handlers
		private bool HandleUnitQueueAction(IClient client, Message msg)
		{
			if (!this.InGame)
				return false;

			var uqa = new Messages.UnitQueueAction(msg);
			if (uqa.Created)
			{
				var token = ((client.UserData as IPlayerData).Player.Type == PlayerType.First ?
					this.GameState.Controller.Player1Queue :
					this.GameState.Controller.Player2Queue).Request(uqa.UnitId);
				client.Send(new Messages.UnitQueued(uqa.UnitId, token != null).ToMessage());
			}
			else
			{
				//TODO: dodać usuwanie z listy
			}
			return true;
		}

		private bool HandlePlayerChangedNick(IClient client, Message msg)
		{
			var pcn = new Messages.PlayerChangedNick(msg);
			pcn.UserId = (client.UserData as IPlayerData).UserId;
			(client.UserData as IPlayerData).Nick = pcn.NewNick;
			this.Server.Clients.SendToAllNoLock(pcn.ToMessage(), c => c != client);
			return true;
		}

		private bool HandlePlayerChangedState(IClient client, Message msg)
		{
			(client.UserData as IPlayerData).ReadyToPlay = !(client.UserData as IPlayerData).ReadyToPlay;
			var pcs = new Messages.PlayerChangedState((client.UserData as IPlayerData).UserId);
			this.Server.Clients.SendToAllNoLock(pcs.ToMessage(), c => c != client);
			this.ReorderPlayersList(client);
			return true;
		}
		#endregion

		#region Other
		private void ReorderPlayersList(IClient player)
		{
			if ((player.UserData as IPlayerData).ReadyToPlay)
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
			this.GameState.Reset();
			//this.GameState.Initialize(new ServerGameConfiguration(
			//    new Player.PlayerInfo(this.ReadyToPlayPlayers[0].Nick, )
			//    ));
		}
		#endregion

		#region Events
		/// <summary>
		/// Wywoływane przez <see cref="IGameState"/> przy dodaniu nowej jednostki.
		/// </summary>
		/// <param name="unit"></param>
		public void UnitAdded(IUnit unit)
		{
			this.Server.Clients.SendToAll(new Messages.UnitCreated(unit.Owner.Type == PlayerType.First ? (byte)0 : (byte)1,
				unit.Description.Id, unit.UnitId, unit.Position).ToMessage());
		}

		/// <summary>
		/// Wywoływane przy usunięciu jednostki z <see cref="IGameState"/>.
		/// </summary>
		/// <param name="unit"></param>
		public void UnitDestroyed(IUnit unit)
		{
			this.Server.Clients.SendToAll(new Messages.UnitDestroyed(unit.Owner.Type == PlayerType.First ? (byte)0 : (byte)1, unit.UnitId).ToMessage());
		}

		/// <summary>
		/// Wywoływane przy dodaniu nowego zasobu przez <see cref="IGameState"/>.
		/// </summary>
		/// <param name="res"></param>
		public void ResourceAdded(IResourceOnMap res)
		{
			this.Server.Clients.SendToAll(new Messages.ResourceAdded(res.Id, res.ResourceId, res.Value, res.Position.X).ToMessage());
		}

		/// <summary>
		/// Wywoływane przy usunięciu zasobu przez <see cref="IGameState"/>.
		/// </summary>
		/// <param name="res"></param>
		public void ResourceRemoved(IResourceOnMap res, IUnit by)
		{
			this.Server.Clients.SendToAll(new Messages.ResourceGathered(res.ResourceId, by.Owner.Type == PlayerType.First ? (byte)0 : (byte)1, by.UnitId).ToMessage());
		}
		#endregion
	}
}
