using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	/// <summary>
	/// Pole tekstowe.
	/// </summary>
	public interface ITextBox
		: IStylizableControl, INotifyPropertyChanged
	{
		/// <summary>
		/// Tekst.
		/// </summary>
		string Text { get; set; }
	}
}
