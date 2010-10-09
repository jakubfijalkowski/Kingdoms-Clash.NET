using System.Drawing;
using OpenTK;

namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Konfiguracja gry.
	/// Powinna być jedna na grę i dostępna z każdego miejsca.
	/// </summary>
	public interface IConfiguration
		: Serialization.IXmlSerializable
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
		Vector2 ScreenSize { get; }

		/// <summary>
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		bool VSync { get; }
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

		/// <summary>
		/// Rozmiary zamku.
		/// </summary>
		Vector2 CastleSize { get; }
		#endregion

		#region Others
		/// <summary>
		/// Czy używać licznika FPS.
		/// </summary>
		bool UseFPSCounter { get; }
		#endregion
	}
}
