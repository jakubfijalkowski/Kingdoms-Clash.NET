namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	/// <summary>
	/// Komponent jednostki określający, że jednostka jest naziemna.
	/// </summary>
	public interface IGroundUnit
		: IUnitComponent
	{
		/// <summary>
		/// Prędkość jednostki.
		/// </summary>
		OpenTK.Vector2 Velocity { get; }
	}
}
