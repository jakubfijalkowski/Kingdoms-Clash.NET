using System.Drawing;

namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Konfiguracja gry.
	/// Powinna być jedna na grę i dostępna z każdego miejsca.
	/// </summary>
	public interface IConfiguration
	{
		#region Window
		/// <summary>
		/// Rozmiar okna.
		/// </summary>
		Size WindowSize { get; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		bool Fullscreen { get; }

		/// <summary>
		/// Rozmiary ekranu.
		/// </summary>
		SizeF ScreenSize { get; }
		#endregion

		#region Gameplay
		/// <summary>
		/// Szybkość poruszania się kamery.
		/// </summary>
		float CameraSpeed { get; }

		/// <summary>
		/// Margines górny dla map.
		/// </summary>
		float MapMargin { get; }

		/// <summary>
		/// Wartość grawitacji.
		/// </summary>
		float Gravity { get; }
		#endregion
	}
}
