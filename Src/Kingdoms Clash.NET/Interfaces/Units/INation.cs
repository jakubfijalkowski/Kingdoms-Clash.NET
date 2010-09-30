namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Player;

	/// <summary>
	/// Nacja.
	/// </summary>
	public interface INation
	{
		/// <summary>
		/// Nazwa nacji.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Dostępne jednostki nacji.
		/// </summary>
		IUnitDescriptionsCollection AvailableUnits { get; }

		/// <summary>
		/// Tworzy jednostkę na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <param name="owner">Właściciel.</param>
		/// <returns>Nowoutworzona jednostka.</returns>
		IUnit CreateUnit(string id, IPlayer owner);
	}
}
