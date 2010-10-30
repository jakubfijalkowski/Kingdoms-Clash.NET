namespace ClashEngine.NET.Interfaces.Gui.Controls
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface ITextButton
		: IGuiControl
	{
		/// <summary>
		/// Czy był wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}