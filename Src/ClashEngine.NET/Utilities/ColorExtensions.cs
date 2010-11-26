using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Utilities
{
	/// <summary>
	/// Rozszerzenia dla klasy koloru.
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		/// Zwraca 4-elementowy wektor przechowujący ten kolor.
		/// </summary>
		/// <param name="color">this</param>
		/// <returns></returns>
		public static Vector4 ToVector4(this Color color)
		{
			return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
		}
	}
}
