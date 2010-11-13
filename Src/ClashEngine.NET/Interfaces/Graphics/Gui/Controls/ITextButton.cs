namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Resources;

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
		/// Czcionka użyta do wyrenderowania tekstu.
		/// </summary>
		IFont Font { get; }

		/// <summary>
		/// Czy był wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}