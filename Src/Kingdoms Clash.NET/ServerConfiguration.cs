using System;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;
	using NET.Interfaces;
	using NET.Interfaces.Player;

	/// <summary>
	/// Konfiguracja serwera.
	/// </summary>
	public class ServerConfiguration
		: IServerConfiguration
	{
		#region Singleton
		private static IServerConfiguration _Instance;

		/// <summary>
		/// Globalna instancja konfiguracji.
		/// </summary>
		public static IServerConfiguration Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new ServerConfiguration();
				}
				return _Instance;
			}
		}
		#endregion

		#region IServerConfiguration Members
		/// <summary>
		/// Port serwera.
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Port informacji.
		/// </summary>
		public int InfoPort { get; set; }

		/// <summary>
		/// Maksymalna liczba widzów.
		/// </summary>
		public uint MaxSpectators { get; set; }

		/// <summary>
		/// Nazwa serwera.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Czas oczekiwania na rozpoczęcie gry.
		/// </summary>
		public TimeSpan WaitTime { get; set; }

		/// <summary>
		/// Typ kontrolera gry.
		/// </summary>
		public Type GameController { get; set; }

		/// <summary>
		/// Typ reguł zwycięstwa.
		/// </summary>
		public Type VictoryRules { get; set; }

		/// <summary>
		/// Ustawienia rozgrywki.
		/// </summary>
		public IGameplaySettings ControllerSettings { get; set; }
		#endregion
	}

	/// <summary>
	/// Ustawienia rozgrywki.
	/// </summary>
	public class ServerGameConfiguration
		: IServerGameConfiguration
	{
		#region IServerGameConfiguration Members
		/// <summary>
		/// Informacje o graczu A.
		/// </summary>
		public IPlayerInfo PlayerA { get; private set; }

		/// <summary>
		/// Informacje o graczu B.
		/// </summary>
		public IPlayerInfo PlayerB { get; private set; }
		#endregion

		#region Constructor
		public ServerGameConfiguration(IPlayerInfo a, IPlayerInfo b)
		{
			this.PlayerA = a;
			this.PlayerB = b;
		}
		#endregion
	}

}
