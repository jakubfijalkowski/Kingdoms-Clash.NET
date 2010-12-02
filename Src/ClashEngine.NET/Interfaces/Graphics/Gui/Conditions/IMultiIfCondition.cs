using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Podwarunek IMultiIf.
	/// </summary>
	/// <remarks>
	/// PropertyChanged musi być wywoływane przy zmianie Value tylko na wartość przeciwną!
	/// </remarks>
	public interface IMultiIfCondition
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Aktualna wartość.
		/// </summary>
		bool Value { get; }
	}
}
