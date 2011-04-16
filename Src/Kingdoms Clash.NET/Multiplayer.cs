using ClashEngine.NET;
using ClashEngine.NET.Interfaces.Net;
using ClashEngine.NET.Net;
using System;
using System.Diagnostics;
using System.Collections.Generic;

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
		private IList<IPlayerData> PlayersInLobby;
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
		public void Remove(IUnit unit)
		{
		}

		/// <summary>
		/// Usuwa zasób z gry.
		/// Musimy zapewnić poprawny przebieg encji, więc dodajemy do kolejki oczekujących na usunięcie.
		/// </summary>
		/// <param name="resource">Zasób.</param>
		public void Remove(IResourceOnMap resource)
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
	}
}
