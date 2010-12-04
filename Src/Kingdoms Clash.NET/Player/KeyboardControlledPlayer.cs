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
		/// Inicjalizuje nowego gracza sterowanego klawiaturą.
		/// </summary>
		/// <param name="name">Nazwa gracza.</param>
		/// <param name="nation">Jegno nacja.</param>
		/// <param name="ucDelay">Czas pomiędzy kolejnymi tworzeniami jednostek.</param>
		public KeyboardControlledPlayer(string name, INation nation)
			: base("Player." + name, name, nation)
		{ }

		public override void OnInit()
		{
			this.Input.KeyChanged += new System.EventHandler<ClashEngine.NET.Interfaces.KeyEventArgs>(Input_KeyChanged);
			base.OnInit();
		}

		void Input_KeyChanged(object sender, ClashEngine.NET.Interfaces.KeyEventArgs e)
		{
			OpenTK.Input.Key startK = OpenTK.Input.Key.Number1, endK = OpenTK.Input.Key.Number9;
			if (this.Type == PlayerType.Second)
			{
				startK = OpenTK.Input.Key.Keypad1;
				endK = OpenTK.Input.Key.Keypad9;
			}

			if (e.IsPressed && e.Key >= startK && e.Key <= endK)
			{
				int unitNo = e.Key - startK;
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
		}
	}
}
