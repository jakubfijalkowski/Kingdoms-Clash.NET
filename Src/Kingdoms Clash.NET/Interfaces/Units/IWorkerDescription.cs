namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Opis/identyfikator jednostki-pracownika.
	/// </summary>
	public interface IWorkerDescription
		: IUnitDescription
	{
		#region Statistics
		/// <summary>
		/// Maksymalny rozmiar ładunku.
		/// </summary>
		int MaxCargoSize { get; }
		#endregion
	}
}
