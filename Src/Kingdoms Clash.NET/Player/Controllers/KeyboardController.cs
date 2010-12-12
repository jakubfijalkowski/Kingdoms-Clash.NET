using System;
using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Player.Controllers
{
	using Interfaces;
	using Interfaces.Player;
	using Interfaces.Player.Controllers;

	/// <summary>
	/// Kontrolowanie gracza za pomocą klawiatury.
	/// </summary>
	/// <remarks>
	/// Wykorzystuje tylko input.
	/// </remarks>
	public class KeyboardController
		: IKeyboardController
	{
		#region IPlayerController Members
		/// <summary>
		/// Gracz, którym kontroler steruje.
		/// </summary>
		public IPlayer Player { get; set; }

		/// <summary>
		/// Stan gry do której należy gracz.
		/// </summary>
		public IGameState GameState { get; set; }

		/// <summary>
		/// Czy kontroler ma pokazywać statystyki.
		/// </summary>
		/// <remarks>
		/// Nieobsługiwane.
		/// </remarks>
		public bool ShowStatistics { get; set; }

		/// <summary>
		/// Inicjalizuje kontroler.
		/// Wywoływane przez IGameState.
		/// </summary>
		/// <param name="screens">Manager ekranów, do którego może się kontroler podpiąć.</param>
		/// <param name="input">Wejście, do którego może się kontroler podpiąć.</param>
		public void Initialize(IScreensManager screens, IInput input)
		{
			input.KeyChanged += new EventHandler<KeyEventArgs>(Input_KeyChanged);
		}
		#endregion

		#region Private methods
		void Input_KeyChanged(object sender, KeyEventArgs e)
		{
			OpenTK.Input.Key startK = OpenTK.Input.Key.Number1, endK = OpenTK.Input.Key.Number9;
			if (this.Player.Type == PlayerType.Second)
			{
				startK = OpenTK.Input.Key.Keypad1;
				endK = OpenTK.Input.Key.Keypad9;
			}

			if (e.IsPressed && e.Key >= startK && e.Key <= endK)
			{
				int unitNo = e.Key - startK;
				int i = 0;
				Interfaces.Units.IUnitDescription ud = null;
				foreach (var u in this.Player.Nation.AvailableUnits)
				{
					ud = u;
					if (i == unitNo)
						break;
				}
				if (i == unitNo)
				{
					this.GameState.Controller[this.Player].Request(ud.Id);
				}
			}
		}
		#endregion
	}
}
