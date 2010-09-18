using ClashEngine.NET.ScreensManager;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Player;
	using Interfaces.Map;
	using Interfaces.Controllers;

	/// <summary>
	/// Główny stan(ekran) gry.
	/// </summary>
	public class GameState
		: Screen, IGameState
	{
		#region IGameState Members
		/// <summary>
		/// Tablica dwóch, aktualnie grających, graczy.
		/// TODO: zaimplementować graczy.
		/// </summary>
		public IPlayer[] Players
		{
			get { throw new System.NotImplementedException(); }
		}

		/// <summary>
		/// Mapa.
		/// TODO: dodać jakąkolwiek mapę.
		/// </summary>
		public IMap Map
		{
			get { throw new System.NotImplementedException(); }
		}

		/// <summary>
		/// Kontroler(tryb) gry.
		/// </summary>
		public IGameController Controller
		{
			get { throw new System.NotImplementedException(); }
		}
		#endregion
	}
}
