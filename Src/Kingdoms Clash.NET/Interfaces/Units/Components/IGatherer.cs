namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	using Interfaces.Resources;

	/// <summary>
	/// Zdolny do zbierania.
	/// </summary>
	public interface IGatherer
		: IUnitComponent
	{
		/// <summary>
		/// Maksymalny rozmiar przenoszonego zasobu.
		/// </summary>
		int MaxCargoSize { get; }

		/// <summary>
		/// Aktualnie przenoszony ładunek lub null, gdy nic nie przenosi.
		/// </summary>
		IResource Cargo { get; set; }
	}
}
