namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Teren 2D.
	/// </summary>
	public interface ITerrain
		: IObject
	{
		/// <summary>
		/// Wysokość terenu.
		/// </summary>
		float Height { get; }
	}
}
