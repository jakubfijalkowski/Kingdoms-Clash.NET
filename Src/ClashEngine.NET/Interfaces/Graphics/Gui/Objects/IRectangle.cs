using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Objects
{
	/// <summary>
	/// Prostokąt.
	/// </summary>
	public interface IRectangle
		: IObject
	{
		/// <summary>
		/// Jego kolor.
		/// </summary>
		Vector4 Color { get; set; }
	}
}
