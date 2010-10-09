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
			Logger.Info("Using default configuration...");
			_Instance = Defaults.DefaultConfiguration;
		}
		#endregion

		#region IConfiguration Members
		/// <summary>
		/// Rozmiar okna.
		/// </summary>
		public Size WindowSize { get; internal set; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		public bool Fullscreen { get; internal set; }

		/// <summary>
		/// Rozmiary ekranu.
		/// </summary>
		public Vector2 ScreenSize { get; internal set; }

		/// <summary>
		/// Czy używać synchronizacji pionowej.
		/// </summary>
		public bool VSync { get; internal set; }

		/// <summary>
		/// Szybkość poruszania się kamery.
		/// </summary>
		public float CameraSpeed { get; internal set; }

		/// <summary>
		/// Margines górny dla map.
		/// </summary>
		public float MapMargin { get; internal set; }

		/// <summary>
		/// Wartość grawitacji.
		/// </summary>
		public float Gravity { get; internal set; }

		/// <summary>
		/// Rozmiary zamku.
		/// </summary>
		public Vector2 CastleSize { get; internal set; }
		#endregion

		internal Configuration()
		{ }

		#region IXmlSerializable Members
		/// <summary>
		/// Serializuje konfiguracje do XML.
		/// </summary>
		/// <param name="element"></param>
		public void Serialize(System.Xml.XmlElement element)
		{
			//Parametry okna
			var window = element.OwnerDocument.CreateElement("window");
			window.SetAttribute("width", this.WindowSize.Width.ToString());
			window.SetAttribute("height", this.WindowSize.Height.ToString());
			window.SetAttribute("fullscreen", this.Fullscreen.ToString().ToLower());
			element.AppendChild(window);

			if (this.VSync)
			{
				element.AppendChild(element.OwnerDocument.CreateElement("vsync"));
			}

			//Ustawiamy resztę
			float aspect = (float)this.WindowSize.Width / (float)this.WindowSize.Height;
			this.ScreenSize = new Vector2(Defaults.DefaultConfiguration.ScreenSize.X, Defaults.DefaultConfiguration.ScreenSize.X / aspect);
			this.CameraSpeed = Defaults.DefaultConfiguration.CameraSpeed;
			this.MapMargin = this.ScreenSize.Y / 2f;
			this.Gravity = Defaults.DefaultConfiguration.Gravity;
			this.CastleSize = Defaults.DefaultConfiguration.CastleSize;
		}

		/// <summary>
		/// Deserializuje konfiguracje.
		/// </summary>
		/// <param name="element"></param>
		public void Deserialize(System.Xml.XmlElement element)
		{
			var window = element["window"];
			if (window == null || !window.HasAttribute("width") || !window.HasAttribute("height") || !window.HasAttribute("fullscreen"))
			{
				throw new System.Xml.XmlException("Missing window element or one of its attributes");
			}
			
			this.WindowSize = new Size(int.Parse(window.GetAttribute("width")), int.Parse(window.GetAttribute("height")));
			this.Fullscreen = bool.Parse(window.GetAttribute("fullscreen"));

			if (element["vsync"] != null)
			{
				this.VSync = true;
			}
		}
		#endregion
	}
}
