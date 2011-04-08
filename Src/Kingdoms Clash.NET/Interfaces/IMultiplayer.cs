using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Server.Interfaces
{
	using NET.Interfaces;

	/// <summary>
	/// Interfejs dla gry multiplayer.
	/// </summary>
	public interface IMultiplayer
	{
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		IGameInfo GameInfo { get; }

		/// <summary>
		/// Uruchamia grę.
		/// </summary>
		void Start();

		/// <summary>
		/// Kończy grę.
		/// </summary>
		void Stop();
	}
}
