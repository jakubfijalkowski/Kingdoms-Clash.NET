namespace Kingdoms_Clash.NET.Player.Controllers
{
	using Interfaces.Player.Controllers;

	/// <summary>
	/// Fabryka dla kontrolerów GUI graczy.
	/// </summary>
	/// <remarks>
	/// Kontrolery są ze sobą ściśle powiązane(są na jednym HUDzie), więc nie można ich tworzyć osobno.
	/// </remarks>
	internal class GuiControllerFactory
		: IGuiControllerFactory
	{
		#region IPlayerControllerFactory Members
		/// <summary>
		/// Manager zasobów wykorzystywany jest do załadowania GUI.
		/// </summary>
		public ClashEngine.NET.Interfaces.IResourcesManager Content { get; set; }

		/// <summary>
		/// Ładuje kontener GUI i udostępnia dwa "dojścia" do niego - jedno jako zaślepkę, drugie, to właściwe, które obsługuje obu graczy.
		/// </summary>
		/// <returns></returns>
		public Interfaces.Player.IPlayerController[] Produce()
		{
			var container = new XAML.PlayersGUIContainer();
			this.Content.Load("Guis/TwoPlayers.xaml", container);

			return new Interfaces.Player.IPlayerController[]
				{
					new Internals.GuiController1(container),
					new Internals.GuiController2(container)
				};
		}
		#endregion
	}
}
