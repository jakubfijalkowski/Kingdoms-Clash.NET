using ClashEngine.NET.ScreensManager;

namespace Kingdoms_Clash.NET
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Stan-ekran gry przy jednym komputerze(niekoniecznie jednego gracza rzeczywistego).
	/// </summary>
	public class SinglePlayer
		: Screen, IGameState, IGameStateScreen
	{
		/// <summary>
		/// Zabezpieczenie na czas tworzenia.
		/// TODO: usunąć.
		/// </summary>
		private bool Initialized = false;

		#region IGameState Members
		#region Properties
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
		/// Inicjalizuje stan gry.
		/// </summary>
		/// <param name="playerA">Pierwszy gracz.</param>
		/// <param name="playerB">Drugi gracz.</param>
		public void Initialize(IPlayer playerA, IPlayer playerB, IMap map, IGameController controller)
		{
			this.Reset();

			this.Players = new IPlayer[] { playerA, playerB };
			this.Map = map;
			this.Controller = controller;

			this.Initialized = true;
		}

		/// <summary>
		/// Resetuje stan gry(zaczyna od początku).
		/// </summary>
		public void Reset()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Dodaje jednostkę do gry.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		public void AddUnit(IUnit unit)
		{
			this.Entities.Add(unit);

			unit.Collide += this.Controller.HandleCollision;
		}
		#endregion
		#endregion

		#region Screen Members
		public override void OnInit()
		{
			if (!this.Initialized)
			{
				throw new System.NotSupportedException("Initialize first");
			}

			this.Controller.GameState = this;

			this.Players[0].GameState = this;
			this.Players[0].Collide += this.Controller.HandleCollision;

			this.Players[1].GameState = this;
			this.Players[1].Collide += this.Controller.HandleCollision;

			this.Entities.Add(this.Map);
			this.Entities.Add(this.Players[0]);
			this.Entities.Add(this.Players[1]);

			//Od tej chwili to kontroler jest odpowiedzialny za wszystko.
		}
		#endregion
	}
}
