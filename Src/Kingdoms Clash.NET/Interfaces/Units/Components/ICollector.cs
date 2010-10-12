namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	/// <summary>
	/// Interfejs dla jednostek, które potrafią zbierać zasoby.
	/// </summary>
	public interface ICollector
		: IUnitComponentDescription
	{
		/// <summary>
		/// Maksymalny rozmiar ładunku, który może unieść jednostka.
		/// </summary>
		uint MaxCargoSize { get; }
	}
}
