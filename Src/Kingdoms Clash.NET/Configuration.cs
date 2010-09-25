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
		#endregion

		#region IConfiguration Members
		/// <summary>
		/// Rozmiar okna.
		/// </summary>
		public Size WindowSize { get; private set; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		public bool Fullscreen { get; set; }

		/// <summary>
		/// Rozmiary ekranu.
		/// </summary>
		public Vector2 ScreenSize { get; private set; }

		/// <summary>
		/// Szybkość poruszania się kamery.
		/// </summary>
		public float CameraSpeed { get; private set; }

		/// <summary>
		/// Margines górny dla map.
		/// </summary>
		public float MapMargin { get; private set; }

		/// <summary>
		/// Wartość grawitacji.
		/// </summary>
		public float Gravity { get; private set; }
		#endregion

		private Configuration()
		{
			Logger.Trace("Loading configuration");
			this.WindowSize = new Size(800, 600);
			this.Fullscreen = false;

			this.ScreenSize = new Vector2(1.0f, 1.0f);
			this.CameraSpeed = 1.0f;

			this.MapMargin = 0.5f;
			this.Gravity = 10f;
		}
	}
}
