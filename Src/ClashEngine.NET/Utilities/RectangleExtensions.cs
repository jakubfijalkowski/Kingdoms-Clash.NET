using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Utilities
{
	/// <summary>
	/// Rozszerzenie klas Rectangle i RectangleF z przestrzeni nazw System.Drawing.
	/// </summary>
	public static class RectangleExtensions
	{
		/// <summary>
		/// Sprawdza, czy dany wektor(punkt) zawiera się w prostokącie.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public static bool Contains(this Rectangle r, Vector2 v)
		{
			return v.X >= r.Left && v.X <= r.Right && v.Y >= r.Top && v.Y <= r.Bottom;
		}

		/// <summary>
		/// Sprawdza, czy dany wektor(punkt) zawiera się w prostokącie.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public static bool Contains(this RectangleF r, Vector2 v)
		{
			return v.X >= r.Left && v.X <= r.Right && v.Y >= r.Top && v.Y <= r.Bottom;
		}
	}
}
