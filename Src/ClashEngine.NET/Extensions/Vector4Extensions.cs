using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Extensions
{
	/// <summary>
	/// Rozszerzenia dla Vector4
	/// </summary>
	public static class Vector4Extensions
	{
		/// <summary>
		/// Zwraca kolor z wektora.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color ToColor(this Vector4 color)
		{
			return Color.FromArgb((int)(color.W * 255f), (int)(color.X * 255f), (int)(color.Y * 255f), (int)(color.Z * 255f));
		}
	}
}
