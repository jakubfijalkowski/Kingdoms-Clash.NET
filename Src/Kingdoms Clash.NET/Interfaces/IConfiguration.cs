using System.Drawing;
using OpenTK;

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
		Size WindowSize { get; set; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		bool Fullscreen { get; set; }

		/// <summary>
		/// Rozmiary ekranu.
		/// </summary>
		Vector2 ScreenSize { get; set; }

		/// <summary>
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		bool VSync { get; set; }
		#endregion

		#region Gameplay
		/// <summary>
		/// Szybkość poruszania się kamery.
		/// </summary>
		float CameraSpeed { get; set; }

		/// <summary>
		/// Margines górny dla map.
		/// </summary>
		float MapMargin { get; set; }

		/// <summary>
		/// Wartość grawitacji.
		/// </summary>
		float Gravity { get; set; }

		/// <summary>
		/// Rozmiary zamku.
		/// </summary>
		Vector2 CastleSize { get; set; }
		#endregion

		#region Others
		/// <summary>
		/// Czy używać licznika FPS.
		/// </summary>
		bool UseFPSCounter { get; set; }

		/// <summary>
		/// Nacja pierwszego gracza.
		/// </summary>
		string Player1Nation { get; set; }

		/// <summary>
		/// Nacja drugiego gracza.
		/// </summary>
		string Player2Nation { get; set; }

		/// <summary>
		/// Ilość zasobów na start.
		/// </summary>
		uint StartResources { get; set; }
		#endregion
	}
}
