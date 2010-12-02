using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface IButton
		: IObjectControl, INotifyPropertyChanged
	{
		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}
