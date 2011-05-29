using System;

namespace Kingdoms_Clash.NET.Player.Controllers
{
	using Interfaces.Player.Controllers;

	/// <summary>
	/// Fabryka dla kontrolerów GUI, gdy gracz .
	/// </summary>
	/// <remarks>
	/// Kontrolery są ze sobą ściśle powiązane(są na jednym HUDzie), więc nie można ich tworzyć osobno.
	/// </remarks>
	internal class NetGuiControllerFactory
		: IGuiControllerFactory
	{
		private Action<string> RequestUnit;

		#region IPlayerControllerFactory Members
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		public ClashEngine.NET.Interfaces.IGameInfo GameInfo { get; set; }

		/// <summary>
		/// Ładuje kontener GUI i udostępnia dwa "dojścia" do niego - jedno jako zaślepkę, drugie, to właściwe, które obsługuje obu graczy.
		/// </summary>
		/// <returns></returns>
		public Interfaces.Player.IPlayerController[] Produce()
		{
			var container = new XAML.PlayersGUIContainer(this.GameInfo, this.RequestUnit);
			this.GameInfo.Content.Load("Guis/TwoPlayers.xml", container);

			//container.Controls["Player2UnitsPanel"].Visible = false; //TODO: wyłączanie obsługi drugiego gracza

			return new Interfaces.Player.IPlayerController[]
				{
					new Internals.GuiController1(container),
					new Internals.GuiController2(container)
				};
		}
		#endregion

		#region Constructor
		public NetGuiControllerFactory(Action<string> requestUnit)
		{
			this.RequestUnit = requestUnit;
		}
		#endregion
	}
}
