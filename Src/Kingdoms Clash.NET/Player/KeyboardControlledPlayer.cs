using ClashEngine.NET;

namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Gracz sterowany klawiaturą.
	/// Czysto testowa.
	/// </summary>
	public class KeyboardControlledPlayer
		: Castle, IHuman
	{
		/// <summary>
		/// Klawisz odpowiedzialny za stworzenie nowej jednostki.
		/// </summary>
		private OpenTK.Input.Key NewUnitKey;

		/// <summary>
		/// Czas pomiędzy kolejnymi tworzeniami jednostek.
		/// </summary>
		private double UnitCreationDelay = 0.5;

		/// <summary>
		/// Akmulator na czas od ostatniego stworzenia jednostki.
		/// </summary>
		private double UnitCreationAccumulator = 0;

		/// <summary>
		/// Identyfikator jednostki do stworzenia.
		/// </summary>
		private string UnitId = "TestUnit";

		/// <summary>
		/// Inicjalizuje nowego gracza sterowanego klawiaturą.
		/// </summary>
		/// <param name="name">Nazwa gracza.</param>
		/// <param name="nation">Jegno nacja.</param>
		/// <param name="newUnitKey">Klawisz odpowiedzialny za stworzenie nowej jednostki.</param>
		/// <param name="ucDelay">Czas pomiędzy kolejnymi tworzeniami jednostek.</param>
		/// <param name="unitId">Jednostka, któa zostanie stworzon.</param>
		public KeyboardControlledPlayer(string name, INation nation, OpenTK.Input.Key newUnitKey, double ucDelay = 0.5, string unitId = "TestUnit")
			: base("Player." + name, name, nation)
		{
			this.NewUnitKey = newUnitKey;
			this.UnitCreationDelay = ucDelay;
			this.UnitId = unitId;
		}

		/// <summary>
		/// Sprawdza czy można stworzyć jednostkę i jeśli tak - tworzy ją.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.UnitCreationAccumulator += delta;
			if (this.UnitCreationAccumulator > this.UnitCreationDelay && Input.Instance.Keyboard[this.NewUnitKey])
			{
				this.GameState.Controller.RequestNewUnit(this.UnitId, this);
				this.UnitCreationAccumulator = 0;
			}

			base.Update(delta);
		}
	}
}
