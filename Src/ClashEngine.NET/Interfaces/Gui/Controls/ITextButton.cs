namespace ClashEngine.NET.Interfaces.Gui.Controls
{
	/// <summary>
	/// Przycisk z tekstem.
	/// </summary>
	public interface ITextButton
		: IGuiControl
	{
		/// <summary>
		/// Tekst na przycisku.
		/// </summary>
		string Label { get; }

		/// <summary>
		/// Czy był wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}