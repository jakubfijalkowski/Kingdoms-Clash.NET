namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Pole tekstowe.
	/// </summary>
	public interface ITextBox
		: IControl
	{
		/// <summary>
		/// Tekst.
		/// </summary>
		string Text { get; set; }
	}
}
