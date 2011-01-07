using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Kolekcja parametrów ICall.
	/// </summary>
	public interface IParametersCollection
		: ICollection<IParameter>
	{
		/// <summary>
		/// Pobiera parametr o wskazanym indeksie.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IParameter this[int index] { get; }
	}
}
