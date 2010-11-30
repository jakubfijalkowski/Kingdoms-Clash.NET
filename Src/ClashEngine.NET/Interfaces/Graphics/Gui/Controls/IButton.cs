using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
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
