using ClashEngine.NET.Interfaces.ScreensManager;
using ClashEngine.NET.ScreensManager;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;

	using Maps;

	/// <summary>
	/// Główny stan(ekran) gry.
	/// </summary>
	public class GameState
		: Screen, IGameState
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		
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
		/// </summary>
		public IMap Map { get; private set; }

		/// <summary>
		/// Kontroler(tryb) gry.
		/// </summary>
		public IGameController Controller { get; private set; }

		/// <summary>
		/// Resetuje stan gry(zaczyna ją od nowa).
		/// </summary>
		public void Reset()
		{
			this.Map.Reset();
			this.Controller.Reset();
		}
		#endregion

		public GameState()
		{
			//Ustawianie właściowści ekranu
			base.IsFullscreen = true;

			Logger.Debug("Creating default game state");
			this.Map = new DefaultMap();
			this.Controller = new Controllers.NormalGame();

			this.Entities.AddEntity(this.Map);
		}

		/// <summary>
		/// Obsługa zmiany stanu.
		/// Jeśli ekran został otwarty na nowo musimy zresetować stan gry.
		/// </summary>
		public override void StateChanged(ScreenState oldState)
		{
			if (this.State != ScreenState.Closed && oldState == ScreenState.Closed)
			{
				this.Reset();
			}
		}
	}
}
