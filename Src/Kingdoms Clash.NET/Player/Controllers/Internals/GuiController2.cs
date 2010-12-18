using ClashEngine.NET.Interfaces;

namespace Kingdoms_Clash.NET.Player.Controllers.Internals
{
	using Interfaces;
	using Interfaces.Player;
	
	/// <summary>
	/// Kontroler GUI - zaślepka.
	/// </summary>
	internal class GuiController2
		: IPlayerController
	{
		#region Private fields
		private XAML.PlayersGUIContainer Container;
		#endregion

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
		public bool ShowStatistics { get; set; }

		/// <summary>
		/// Inicjalizuje kontroler.
		/// Wywoływane przez IGameState.
		/// </summary>
		/// <param name="screens">Manager ekranów, do którego może się kontroler podpiąć.</param>
		/// <param name="input">Wejście, do którego może się kontroler podpiąć.</param>
		public void Initialize(IScreensManager screens, IInput input)
		{
			this.Container.Player2 = this.Player;
			this.Container.Player2Queue = this.GameState.Controller.Player2Queue;
		}
		#endregion

		#region Constructors
		public GuiController2(XAML.PlayersGUIContainer container)
		{
			this.Container = container;
		}
		#endregion
	}
}
