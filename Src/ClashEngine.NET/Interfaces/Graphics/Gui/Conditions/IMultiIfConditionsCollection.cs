using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Typ warunku.
	/// </summary>
	public enum MultiIfConditionType
	{
		/// <summary>
		/// I - &&
		/// </summary>
		And,
		
		/// <summary>
		/// Lub - ||
		/// </summary>
		Or
	}

	/// <summary>
	/// Kolekcja podwarunków.
	/// </summary>
	public interface IMultiIfConditionsCollection
		: ICollection<IMultiIfCondition>, IMultiIfCondition
	{
		/// <summary>
		/// Typ łączenia warunków.
		/// </summary>
		MultiIfConditionType Type { get; set; }
	}
}
