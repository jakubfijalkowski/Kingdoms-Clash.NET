using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Interfaces.Factories
{
	/// <summary>
	/// Interfejs dla fabryki kontrolerów graczy.
	/// </summary>
	public interface IPlayerControllerFactory
	{
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		IGameInfo GameInfo { get; set; }

		/// <summary>
		/// Produkuje dwa kontrolery graczy.
		/// </summary>
		/// <returns>Tablica dwóch obiektów, które są, odpowiednio, kontrolerem pierwszego i drugiego gracza.</returns>
		Player.IPlayerController[] Produce();
	}
}
