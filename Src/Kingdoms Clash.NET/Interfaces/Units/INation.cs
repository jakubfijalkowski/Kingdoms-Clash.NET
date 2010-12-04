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
		/// Ścieżka do obrazka zamku.
		/// </summary>
		string CastleImage { get; }

		/// <summary>
		/// Dostępne jednostki nacji.
		/// </summary>
		IUnitDescriptionsCollection AvailableUnits { get; }
	}
}
