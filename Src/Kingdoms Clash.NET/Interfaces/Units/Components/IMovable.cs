namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	/// <summary>
	/// Interfejs określa jednostki które potrafią się poruszać.
	/// </summary>
	public interface IMovable
		: IUnitComponent
	{
		/// <summary>
		/// Szybkość poruszania się jednostki.
		/// </summary>
		float Speed { get; }
	}
}
