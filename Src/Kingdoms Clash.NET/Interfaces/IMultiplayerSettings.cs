using System.Net;

namespace Kingdoms_Clash.NET.Interfaces
{
	using Interfaces.Controllers;

	/// <summary>
	/// Ustawienia gry dla wielu graczy.
	/// </summary>
	public interface IMultiplayerSettings
	{
		/// <summary>
		/// Adres serwera.
		/// </summary>
		IPAddress Address { get; set; }

		/// <summary>
		/// Numer portu.
		/// </summary>
		int Port { get; set; }

		/// <summary>
		/// Kontroler lokalny gracza.
		/// </summary>
		IGameController LocalPlayerController { get; set; } //?

		/// <summary>
		/// Nick gracza.
		/// </summary>
		string PlayerNick { get; set; }
	}
}
