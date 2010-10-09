namespace Kingdoms_Clash.NET.Interfaces.Units.Components
{
	/// <summary>
	/// Jednostka posiada obrazek(sprite).
	/// </summary>
	public interface ISprite
		: IUnitComponentDescription
	{
		/// <summary>
		/// Ścieżka do obrazka
		/// </summary>
		string ImagePath { get; }
	}
}
