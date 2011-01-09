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
		/// Rozmiar viewportu po przekształceniach kamery.
		/// </summary>
		new OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Zakres widoczności kamery.
		/// </summary>
		RectangleF Borders { get; set; }

		/// <summary>
		/// Aktualna pozycja kamery.
		/// </summary>
		Vector2 CurrentPosition { get; set; }

		/// <summary>
		/// Zmienia(równolegle) rozmiar i zakres widoczności kamery.
		/// </summary>
		/// <param name="size">Rozmiar.</param>
		/// <param name="borders">Zakres widoczności.</param>
		void Change(Vector2 size, RectangleF borders);
	}
}
