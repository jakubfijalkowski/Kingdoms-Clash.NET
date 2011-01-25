namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	/// <summary>
	/// Komponent jednostki określający, że jednostka potrafi się poruszać.
	/// </summary>
	public interface IMovable
		: IUnitComponentDescription
	{
		/// <summary>
		/// Maksymalna prędkość jednostki.
		/// </summary>
		float MaxVelocity { get; }

		/// <summary>
		/// Siła, jaka działa na jednostkę.
		/// </summary>
		float Force { get; }
	}
}
