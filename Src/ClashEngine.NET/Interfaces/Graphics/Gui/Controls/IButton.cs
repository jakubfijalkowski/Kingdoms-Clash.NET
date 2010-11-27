namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface IButton
		: IControl, System.ComponentModel.INotifyPropertyChanged
	{
		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		bool Clicked { get; }
	}
}
