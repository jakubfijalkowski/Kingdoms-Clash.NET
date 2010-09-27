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
		private const float ScreenWidth = 100.0f;

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

			float aspect = (float)this.WindowSize.Width / this.WindowSize.Height;

			this.ScreenSize = new Vector2(ScreenWidth, ScreenWidth / aspect);
			this.CameraSpeed = 100.0f;

			this.MapMargin = this.ScreenSize.Y / 2f;
			this.Gravity = 300f;
		}
	}
}
