namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	/// <summary>
	/// Statystyki produkcji jednostek.
	/// </summary>
	public interface IUnitQueueStats
	{
		/// <summary>
		/// Aktualnie produkowana jednostka.
		/// </summary>
		Units.IUnitRequestToken CurrentToken { get; }

		/// <summary>
		/// Długość kolejki.
		/// </summary>
		uint QueueLength { get; }
	}
}
