using System.Net;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Controllers.Victory;
	using Interfaces.Map;
	using Interfaces.Player;

	/// <summary>
	/// Ustawienia gry jednoosobowej.
	/// </summary>
	public class SingleplayerSettings
		: ISingleplayerSettings
	{
		#region IGameSettings Members
		/// <summary>
		/// Informacje o graczu A.
		/// </summary>
		public IPlayerInfo PlayerA { get; set; }

		/// <summary>
		/// Informacje o graczu B.
		/// </summary>
		public IPlayerInfo PlayerB { get; set; }

		/// <summary>
		/// Informacje o mapie.
		/// </summary>
		public IMap Map { get; set; }

		/// <summary>
		/// Kontroler gry.
		/// </summary>
		public IGameController Controller { get; set; }

		/// <summary>
		/// Reguły wygranej dla tej gry.
		/// </summary>
		public IVictoryRules VictoryRules { get; set; }

		/// <summary>
		/// Ustawienia rozgrywki, są one zależne od kontrolera, który grę obsługuje.
		/// </summary>
		public IGameplaySettings Gameplay { get; set; }
		#endregion
	}

	/// <summary>
	/// Ustawienia gry wieloosobowej.
	/// </summary>
	public class MultiplayerSettings
		: IMultiplayerSettings
	{
		#region IGameSettings Members
		/// <summary>
		/// Adres serwera.
		/// </summary>
		public IPAddress Address { get; set; }

		/// <summary>
		/// Numer portu.
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Kontroler lokalny gracza.
		/// </summary>
		public IGameController LocalPlayerController { get; set; }

		/// <summary>
		/// Nick gracza.
		/// </summary>
		public string PlayerNick { get; set; }
		#endregion
	}
}
