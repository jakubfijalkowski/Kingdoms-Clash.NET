using System;

namespace Kingdoms_Clash.NET.Server.Interfaces
{
	using NET.Interfaces;

	/// <summary>
	/// Konfiguracja serwera.
	/// </summary>
	public interface IServerConfiguration
	{
		/// <summary>
		/// Port serwera.
		/// </summary>
		int Port { get; set; }

		/// <summary>
		/// Port informacji.
		/// </summary>
		int InfoPort { get; set; }

		/// <summary>
		/// Maksymalna liczba widzów.
		/// </summary>
		uint MaxSpectators { get; set; }

		/// <summary>
		/// Nazwa serwera.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Czas oczekiwania na rozpoczęcie gry.
		/// </summary>
		TimeSpan WaitTime { get; set; }

		/// <summary>
		/// Typ kontrolera gry.
		/// </summary>
		Type GameController { get; set; }

		/// <summary>
		/// Typ reguł zwycięstwa.
		/// </summary>
		Type VictoryRules { get; set; }

		/// <summary>
		/// Ustawienia rozgrywki.
		/// </summary>
		IGameplaySettings ControllerSettings { get; set; }
	}
}
