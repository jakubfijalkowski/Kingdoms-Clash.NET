using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	/// <summary>
	/// Przycisk.
	/// </summary>
	public interface IButton
		: IStylizableControl, INotifyPropertyChanged
	{
		/// <summary>
		/// Lista wciśniętych przycisków.
		/// </summary>
		/// <remarks>
		/// Poszczególne bity odpowiadają stanowi klawisza z MouseButton.
		/// Bit 12(LastButton) odpowiada puszczeniu lewego klawisza nad przyciskiem.
		/// </remarks>
		ushort ClickedButtons { get; }
	}
}
