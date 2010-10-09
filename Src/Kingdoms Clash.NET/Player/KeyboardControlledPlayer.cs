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
		/// Czas pomiędzy kolejnymi tworzeniami jednostek.
		/// </summary>
		private double UnitCreationDelay = 0.5;

		/// <summary>
		/// Akmulator na czas od ostatniego stworzenia jednostki.
		/// </summary>
		private double UnitCreationAccumulator = 0;

		/// <summary>
		/// Inicjalizuje nowego gracza sterowanego klawiaturą.
		/// </summary>
		/// <param name="name">Nazwa gracza.</param>
		/// <param name="nation">Jegno nacja.</param>
		/// <param name="newUnitKey">Klawisz odpowiedzialny za stworzenie nowej jednostki.</param>
		/// <param name="ucDelay">Czas pomiędzy kolejnymi tworzeniami jednostek.</param>
		/// <param name="unitId">Jednostka, któa zostanie stworzon.</param>
		public KeyboardControlledPlayer(string name, INation nation, double ucDelay = 0.5)
			: base("Player." + name, name, nation)
		{
			this.UnitCreationDelay = ucDelay;
		}

		/// <summary>
		/// Sprawdza czy można stworzyć jednostkę i jeśli tak - tworzy ją.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.UnitCreationAccumulator += delta;
			if (this.UnitCreationAccumulator > this.UnitCreationDelay/* && Input.Instance.Keyboard[this.NewUnitKey]*/)
			{
				int unitNo = -1;
				if (this.Type == PlayerType.First)
				{
					for (OpenTK.Input.Key i = OpenTK.Input.Key.Number1; i <= OpenTK.Input.Key.Number9; i++)
					{
						if (Input.Instance.Keyboard[i])
						{
							unitNo = i - OpenTK.Input.Key.Number1;
						}
					}
				}
				else
				{
					for (OpenTK.Input.Key i = OpenTK.Input.Key.Keypad1; i <= OpenTK.Input.Key.Keypad9; i++)
					{
						if (Input.Instance.Keyboard[i])
						{
							unitNo = i - OpenTK.Input.Key.Keypad1;
						}
					}
				}
				if (unitNo != -1)
				{
					int i = 0;
					IUnitDescription ud = null;
					foreach (var u in this.Nation.AvailableUnits)
					{
						ud = u;
						if (i == unitNo)
							break;
					}
					if (i == unitNo)
					{
						this.GameState.Controller.RequestNewUnit(ud.Id, this);
					}
				}
				this.UnitCreationAccumulator = 0;
			}

			base.Update(delta);
		}
	}
}
