using System.Drawing;
using OpenTK;

namespace Kingdoms_Clash.NET
{
	/// <summary>
	/// Konfiguracja gry.
	/// </summary>
	public class Configuration
		: Interfaces.IConfiguration
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Singleton
		private static Interfaces.IConfiguration _Instance;

		/// <summary>
		/// Globalna instancja konfiguracji.
		/// </summary>
		public static Interfaces.IConfiguration Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new Configuration();
				}
				return _Instance;
			}
		}

		/// <summary>
		/// Wymusza użycie domyślnej konfiguracji.
		/// </summary>
		public static void UseDefault()
		{
			Logger.Info("Using default configuration");
			_Instance = Defaults.DefaultConfiguration;
		}
		#endregion

		#region IConfiguration Members
		/// <summary>
		/// Rozmiar okna.
		/// </summary>
		public Size WindowSize { get; set; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		public bool Fullscreen { get; set; }

		/// <summary>
		/// Rozmiary ekranu.
		/// </summary>
		public Vector2 ScreenSize { get; set; }

		/// <summary>
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		public bool VSync { get; set; }

		/// <summary>
		/// Szybkość poruszania się kamery.
		/// </summary>
		public float CameraSpeed { get; set; }

		/// <summary>
		/// Margines górny dla map.
		/// </summary>
		public float MapMargin { get; set; }

		/// <summary>
		/// Wartość grawitacji.
		/// </summary>
		public float Gravity { get; set; }

		/// <summary>
		/// Rozmiary zamku.
		/// </summary>
		public Vector2 CastleSize { get; set; }

		/// <summary>
		/// Czy używać licznika FPS.
		/// </summary>
		public bool UseFPSCounter { get; set; }

		/// <summary>
		/// Nacja pierwszego gracza.
		/// </summary>
		public string Player1Nation { get; set; }

		/// <summary>
		/// Nacja drugiego gracza.
		/// </summary>
		public string Player2Nation { get; set; }

		/// <summary>
		/// Ilość zasobów na start.
		/// </summary>
		public uint StartResources { get; set; }
		#endregion

		internal Configuration()
		{ }
	}
}
