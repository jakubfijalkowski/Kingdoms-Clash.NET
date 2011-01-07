using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Parametr dla ICall.
	/// </summary>
	public interface IParameter
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Wartość.
		/// </summary>
		object Value { get; set; }
	}
}
