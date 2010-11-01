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
		/// <param name="ucDelay">Czas pomiędzy kolejnymi tworzeniami jednostek.</param>
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

				OpenTK.Input.Key startK = OpenTK.Input.Key.Number1, endK = OpenTK.Input.Key.Number9;
				if (this.Type == PlayerType.Second)
				{
					startK = OpenTK.Input.Key.Keypad1;
					endK = OpenTK.Input.Key.Keypad9;
				}

				for (OpenTK.Input.Key i = startK; i <= endK; i++)
				{
					if (this.Input[i])
					{
						unitNo = i - startK;
						break;
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
