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
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		bool VSync { get; set; }
		#endregion

		#region Gameplay
		/// <summary>
		/// Szybkość poruszania się kamery.
		/// </summary>
		float CameraSpeed { get; set; }
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
		#endregion

		/// <summary>
		/// Kopiuje wartości konfiguracji z innej.
		/// </summary>
		/// <param name="other">Inna.</param>
		void Set(IConfiguration other);
	}
}
