using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Utilities
{
	/// <summary>
	/// Rozszerzenia Vector2.
	/// </summary>
	public static class Vector2Extensions
	{
		/// <summary>
		/// Sprawdza czy dany punkt jest w prostokącie.
		/// </summary>
		/// <param name="vec">this</param>
		/// <param name="rect">Prostokąt.</param>
		/// <returns></returns>
		public static bool IsIn(this Vector2 vec, RectangleF rect)
		{
			return rect.Contains(vec);
		}

		/// <summary>
		/// Sprawdza czy dany punkt jest w prostokącie.
		/// </summary>
		/// <param name="vec">this</param>
		/// <param name="position">Pozycja prostokąta.</param>
		/// <param name="position">Rozmiar prostokąta.</param>
		/// <returns></returns>
		public static bool IsIn(this Vector2 vec, Vector2 position, Vector2 size)
		{
			return vec.X >= position.X && vec.X <= position.X + size.X &&
				vec.Y >= position.Y && vec.Y <= position.Y + size.Y;
		}
	}
}
