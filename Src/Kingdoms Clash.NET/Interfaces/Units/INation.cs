using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Player;

	/// <summary>
	/// Nacja.
	/// </summary>
	public interface INation
	{
		/// <summary>
		/// Dostępne jednostki nacji.
		/// </summary>
		IList<IUnitDescription> AvailableUnits { get; }

		/// <summary>
		/// Tworzy jednostkę na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <param name="owner">Właściciel.</param>
		/// <returns>Nowoutworzona jednostka.</returns>
		IUnit CreateUnit(IUnitDescription id, IPlayer owner);
	}
}
