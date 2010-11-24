using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Cameras
{
	/// <summary>
	/// Kamer 2D, którą można przesuwać.
	/// </summary>
	public interface IMovable2DCamera
		: ICamera
	{
		/// <summary>
		/// Zakres widoczności kamery.
		/// </summary>
		RectangleF Borders { get; }

		/// <summary>
		/// Aktualna pozycja kamery.
		/// </summary>
		Vector2 CurrentPosition { get; set; }
	}
}
