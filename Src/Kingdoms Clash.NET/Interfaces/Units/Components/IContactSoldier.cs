namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	/// <summary>
	/// Interfejs dla komponentów jednostek które walczą wręcz.
	/// </summary>
	public interface IContactSoldier
		: IUnitComponentDescription
	{
		/// <summary>
		/// Siła jednostki.
		/// </summary>
		int Strength { get; }
	}
}
