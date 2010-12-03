namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Interfejs opisu komponentu.
	/// </summary>
	public interface IUnitComponentDescription
	{
		/// <summary>
		/// Tworzy komponent na podstawie opisu.
		/// </summary>
		/// <returns>Komponent.</returns>
		IUnitComponent Create();
	}
}
