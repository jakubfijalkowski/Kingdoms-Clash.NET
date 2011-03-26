using System;

namespace Kingdoms_Clash.NET.Server
{
	using Interfaces;

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
		/// Maksymalna liczba widzów.
		/// </summary>
		public uint MaxSpectators { get; set; }

		/// <summary>
		/// Nazwa serwera.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Typ kontrolera gry.
		/// </summary>
		public Type GameController { get; set; }

		/// <summary>
		/// Typ reguł zwycięstwa.
		/// </summary>
		public Type VictoryRules { get; set; }
		#endregion
	}
}
