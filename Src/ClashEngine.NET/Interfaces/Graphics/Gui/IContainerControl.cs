namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontrolka-kontener, posiada listę kontrolek-dzieci.
	/// </summary>
	/// <seealso cref="IContainer"/>
	public interface IContainerControl
		: IControl
	{
		/// <summary>
		/// Kolekcja kontrolek.
		/// </summary>
		IControlsCollection Controls { get; }

		/// <summary>
		/// Sprawdza stan kontrolki za pomocą <see cref="IGuiControl.Check"/>.
		/// </summary>
		/// <param name="id">Identyfikator kontrolki.</param>
		/// <returns>Nr akcji bądź 0, gdy żadna akcja nie zaszła.</returns>
		int Control(string id);
	}
}
