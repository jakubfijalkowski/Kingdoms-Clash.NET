using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Nacja.
	/// </summary>
	public interface INation
	{
		/// <summary>
		/// Dostępne jednostki nacji.
		/// </summary>
		IList<IUnit> AvailableUnits { get; }
	}
}
