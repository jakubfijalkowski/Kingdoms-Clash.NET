using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ClashEngine.NET;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Stan-ekran gry wieloosobowej.
	/// </summary>
	public class Multiplayer
		: Screen, IMultiplayer, IGameStateScreen
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Private
		private uint UserId;
		private IClient Client;
		private IMultiplayerSettings MainSettings;
		private List<IPlayerData> PlayersInLobby;
		private bool InGame = false;
		#endregion

		#region IGameState Members
		#region Properties
		/// <summary>
		/// Ustawienia gry.
		/// </summary>
		public IGameplaySettings Settings { get; private set; }

		/// <summary>
		/// Tablica dwóch, aktualnie grających, graczy.
		/// </summary>
		public IPlayer[] Players { get; private set; }

		/// <summary>
		/// Mapa.
		/// </summary>
		public IMap Map { get; private set; }

		/// <summary>
		/// Kontroler(tryb) gry.
		/// </summary>
		public IGameController Controller { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		public void Reset()
		{
		}

		/// <summary>
		/// Dodaje jednostkę do gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		public void Add(IUnit unit)
		{
		}

		/// <summary>
		/// Dodaje zasób do gry.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		public void Add(IResourceOnMap resource)
		{
		}

		/// <summary>
		/// Usuwa jednostkę z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		public void Kill(IUnit unit)
		{
		}

		/// <summary>
		/// Usuwa zasób z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		public void Gather(IResourceOnMap resource, IUnit by)
		{
		}
		#endregion
		#endregion

		#region IMultiplayer Members
		/// <summary>
		/// Inicjalizuje stan gry.
		/// </summary>
		/// <param name="settings">Ustawienia gry.</param>
		public void Initialize(IMultiplayerSettings settings)
		{
			this.MainSettings = settings;
		}
		#endregion

		#region Screen Members
		public override void OnInit()
		{
			this.Client = new TcpClient(new System.Net.IPEndPoint(this.MainSettings.Address, this.MainSettings.Port),
				System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

			this.Client.Open();
			if (this.Client.Status != ClientStatus.Ok)
			{
				throw new Exception("Cannot connect to server - " + this.Client.Status.ToString()); //TODO: ładne przedstawienie błędu
			}

			Messages.PlayersFirstConfiguration firstMsg = new Messages.PlayersFirstConfiguration(this.MainSettings.PlayerNick);
			this.Client.Send(firstMsg.ToMessage());

			Stopwatch wait = new Stopwatch();
			wait.Start();
			while (wait.Elapsed.TotalMinutes < 1 && !this.Client.Messages.Contains((MessageType)GameMessageType.PlayerAccepted)) ;
			wait.Stop();
			if (this.Client.Messages.Contains((MessageType)GameMessageType.PlayerAccepted))
			{
				var msg = new Messages.PlayerAccepted(this.Client.Messages.GetFirst((MessageType)GameMessageType.PlayerAccepted));
				this.UserId = msg.UserId;
				this.PlayersInLobby = msg.Players;
				this.Client.Send(new Messages.PlayerChangedState(this.UserId).ToMessage());
			}
			else
			{
				throw new Exception("Cannot connect to server - " + (this.Client.Status == ClientStatus.Ok ? "not responding" : this.Client.Status.ToString())); //TODO: ładne przedstawienie błędu
			}
			Logger.Info("Players in lobby: {0}", this.PlayersInLobby.Count);
			foreach (var p in this.PlayersInLobby)
			{
				Logger.Info("\t{0}", p.Nick);
			}
		}

		public override void OnDeinit()
		{
			this.Client.Close();
		}

		public override void Update(double delta)
		{
			this.ProcessOther();
			if (this.InGame)
				this.ProcessInGame();
			else
				this.ProcessNonInGame();
		}

		public override void Render()
		{
		}
		#endregion

		#region Constructors
		public Multiplayer()
			: base("GameScreen", ClashEngine.NET.Interfaces.ScreenType.Fullscreen)
		{ }
		#endregion

		#region Handling
		/// <summary>
		/// Obsługa w trakcie gry.
		/// </summary>
		private void ProcessInGame()
		{

		}

		/// <summary>
		/// Obsługa poza grą.
		/// </summary>
		private void ProcessNonInGame()
		{
			if (this.Client.Messages.Count == 0)
				return;

			switch ((GameMessageType)this.Client.Messages[0].Type)
			{
				case GameMessageType.GameWillStartAfter:
					{
						var msg = new Messages.GameWillStartAfter(this.Client.Messages[0]);
						this.Client.Messages.RemoveAt(0);
						Logger.Info("Game will start after {0}", msg.Time.ToString());
					}
					break;
			}
		}

		/// <summary>
		/// Obsługa pozostałych.
		/// </summary>
		private void ProcessOther()
		{
			if (this.Client.Messages.Count == 0)
				return;
			//TODO: błędne wiadomości
			switch ((GameMessageType)this.Client.Messages[0].Type)
			{
				case GameMessageType.PlayerConnected:
					{
						Messages.PlayerConnected newPlayer = new Messages.PlayerConnected(this.Client.Messages[0]);
						this.PlayersInLobby.Add(new Player.PlayerData(newPlayer.UserId) { Nick = newPlayer.Nick });
						Logger.Info("Player {0} connected", newPlayer.Nick);
						this.Client.Messages.RemoveAt(0);
					}
					break;
				case GameMessageType.PlayerDisconnected:
					{
						Messages.PlayerDisconnected disconnected = new Messages.PlayerDisconnected(this.Client.Messages[0]);
						int idx = this.PlayersInLobby.FindIndex(p => p.UserId == disconnected.UserId);
						Logger.Info("Player {0} disconnected, reason: {1}", this.PlayersInLobby[idx].Nick, disconnected.Reason.ToString());
						this.PlayersInLobby.RemoveAt(0);
					}
					break;
				case GameMessageType.PlayerChangedNick:
					{
						Messages.PlayerChangedNick newNick = new Messages.PlayerChangedNick(this.Client.Messages[0]);
						int idx = this.PlayersInLobby.FindIndex(p => p.UserId == newNick.UserId);
						Logger.Info("Player {0} changed the nick to {1}", this.PlayersInLobby[idx].Nick, newNick.NewNick);
						this.PlayersInLobby[idx].Nick = newNick.NewNick;
						this.Client.Messages.RemoveAt(0);
					}
					break;
				case GameMessageType.PlayerChangedState:
					{
						Messages.PlayerChangedState newState = new Messages.PlayerChangedState(this.Client.Messages[0]);
						int idx = this.PlayersInLobby.FindIndex(p => p.UserId == newState.UserId);
						this.PlayersInLobby[idx].ReadyToPlay = !this.PlayersInLobby[idx].ReadyToPlay;
						Logger.Info("Player {0} changed his state to {1}", this.PlayersInLobby[idx].Nick,
							(this.PlayersInLobby[idx].ReadyToPlay ? "ready-to-play" : "spectator"));
						this.Client.Messages.RemoveAt(0);
					}
					break;
			}
		}
		#endregion
	}
}
