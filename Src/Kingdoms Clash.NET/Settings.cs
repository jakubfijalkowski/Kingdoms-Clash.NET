using System;
using OpenTK;

namespace Kingdoms_Clash.NET
{
	/// <summary>
	/// Klasa z ustawieniami stałymi - użytkownik nie może ich zmieniać.
	/// </summary>
	public static class Settings
	{
		/// <summary>
		/// Wartość grawitacji.
		/// </summary>
		public static readonly float Gravity = 200f;

		/// <summary>
		/// Rozmiary zamku.
		/// </summary>
		public static readonly Vector2 CastleSize = new Vector2(20f, 30f);

		/// <summary>
		/// Margines górny dla map.
		/// </summary>
		public static readonly float MapMargin = 75f / 2f;

		/// <summary>
		/// Widoczna część gry.
		/// Jest to tylko szerokość, wysokość należy wyliczyć uwzględniając aspekt okna.
		/// </summary>
		public static readonly float ScreenSize = 100f;

		/// <summary>
		/// Co ile gra będzie informować o zmianie czasu do rozpoczęcia gry.
		/// </summary>
		public static readonly TimeSpan WaitTimeDelay = new TimeSpan(0, 0, 10);
	}
}
