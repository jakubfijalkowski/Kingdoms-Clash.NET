using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Extensions
{
	/// <summary>
	/// Rozszerzenie klas Rectangle i RectangleF z przestrzeni nazw System.Drawing.
	/// </summary>
	public static class RectangleExtensions
	{
		/// <summary>
		/// Sprawdza, czy dany wektor(punkt) zawiera się w prostokącie.
		/// </summary>
		/// <param name="r">this</param>
		/// <param name="v">Punkt.</param>
		/// <returns></returns>
		public static bool Contains(this Rectangle r, Vector2 v)
		{
			return v.X >= r.Left && v.X <= r.Right && v.Y >= r.Top && v.Y <= r.Bottom;
		}

		/// <summary>
		/// Sprawdza, czy dany wektor(punkt) zawiera się w prostokącie.
		/// </summary>
		/// <param name="r">this</param>
		/// <param name="v">Punkt.</param>
		/// <returns></returns>
		public static bool Contains(this RectangleF r, Vector2 v)
		{
			return v.X >= r.Left && v.X <= r.Right && v.Y >= r.Top && v.Y <= r.Bottom;
		}

		/// <summary>
		/// Zwraca lewy górny róg prostokąta.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static Vector2 TopLeft(this RectangleF r)
		{
			return new Vector2(r.Left, r.Top);
		}

		/// <summary>
		/// Zwraca prawy górny róg prostokąta.
		/// </summary>
		/// <param name="r">this</param>
		/// <returns></returns>
		public static Vector2 TopRight(this RectangleF r)
		{
			return new Vector2(r.Right, r.Top);
		}


		/// <summary>
		/// Zwraca lewy dolny róg prostokąta.
		/// </summary>
		/// <param name="r">this</param>
		/// <returns></returns>
		public static Vector2 BottomLeft(this RectangleF r)
		{
			return new Vector2(r.Left, r.Bottom);
		}


		/// <summary>
		/// Zwraca prawy dolny róg prostokąta.
		/// </summary>
		/// <param name="r">this</param>
		/// <returns></returns>
		public static Vector2 BottomRight(this RectangleF r)
		{
			return new Vector2(r.Right, r.Bottom);
		}

		/// <summary>
		/// Pobiera rozmiar prostokąta.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static Vector2 GetSize(this RectangleF r)
		{
			return new Vector2(r.Width, r.Height);
		}
	}
}
