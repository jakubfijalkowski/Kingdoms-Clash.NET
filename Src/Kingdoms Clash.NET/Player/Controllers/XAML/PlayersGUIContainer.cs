using ClashEngine.NET.Graphics.Gui;

namespace Kingdoms_Clash.NET.Player.Controllers.XAML
{
	using Interfaces.Controllers;

	/// <summary>
	/// Kontener GUI dla kontrolera gracza.
	/// </summary>
	public class PlayersGUIContainer
		: XamlGuiContainer
	{
		/// <summary>
		/// Pierwszy gracz.
		/// </summary>
		public Interfaces.Player.IPlayer Player1 { get; internal set; }

		/// <summary>
		/// Drugi gracz.
		/// </summary>
		public Interfaces.Player.IPlayer Player2 { get; internal set; }

		/// <summary>
		/// Kolejka jednostek pierwszego gracza.
		/// </summary>
		public IUnitQueue Player1Queue { get; internal set; }

		/// <summary>
		/// Kolejka jednostek drugiego gracza.
		/// </summary>
		public IUnitQueue Player2Queue { get; internal set; }
	}
}
