using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface IButton
		: IControl, INotifyPropertyChanged
	{
		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}
