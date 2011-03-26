using System.Xml;

namespace Kingdoms_Clash.NET.UserData
{
	/// <summary>
	/// Klasa serializera konfiguracji.
	/// </summary>
	internal class ConfigurationSerializer
	{
		/// <summary>
		/// Obiekt konfiguracji do serializacji/deserializacji.
		/// </summary>
		public Interfaces.IConfiguration Configuration { get; set; }

		#region IXmlSerializable Members
		/// <summary>
		/// Serializuje obiekt Configuration do XML.
		/// </summary>
		/// <param name="element"></param>
		public void Serialize(XmlElement element)
		{
			//Parametry okna
			var window = element.OwnerDocument.CreateElement("window");
			window.SetAttribute("width", this.Configuration.WindowSize.Width.ToString());
			window.SetAttribute("height", this.Configuration.WindowSize.Height.ToString());
			window.SetAttribute("fullscreen", this.Configuration.Fullscreen.ToString().ToLower());
			element.AppendChild(window);

			if (this.Configuration.VSync)
			{
				element.AppendChild(element.OwnerDocument.CreateElement("vsync"));
			}

			if (this.Configuration.UseFPSCounter)
			{
				element.AppendChild(element.OwnerDocument.CreateElement("fpscounter"));
			}

			var player1 = element.OwnerDocument.CreateElement("player1");
			player1.SetAttribute("nation", this.Configuration.Player1Nation);
			element.AppendChild(player1);

			var player2 = element.OwnerDocument.CreateElement("player2");
			player2.SetAttribute("nation", this.Configuration.Player2Nation);
			element.AppendChild(player2);

			var camera = element.OwnerDocument.CreateElement("camera");
			camera.SetAttribute("speed", this.Configuration.CameraSpeed.ToString());
			element.AppendChild(camera);
		}

		/// <summary>
		/// Deserializuje XML do obiektu Configuration.
		/// </summary>
		/// <param name="element"></param>
		public void Deserialize(XmlElement element)
		{
			this.Configuration.Set(Defaults.DefaultClientConfiguration);

			var window = element["window"];
			if (window == null || !window.HasAttribute("width") || !window.HasAttribute("height") || !window.HasAttribute("fullscreen"))
			{
				throw new System.Xml.XmlException("Missing window element or one of its attributes");
			}

			this.Configuration.WindowSize = new System.Drawing.Size(int.Parse(window.GetAttribute("width")), int.Parse(window.GetAttribute("height")));
			this.Configuration.Fullscreen = bool.Parse(window.GetAttribute("fullscreen"));

			if (element["vsync"] != null)
			{
				this.Configuration.VSync = true;
			}

			if (element["fpscounter"] != null)
			{
				this.Configuration.UseFPSCounter = true;
			}

			var player1 = element["player1"];
			if (player1 != null && player1.HasAttribute("nation"))
			{
				this.Configuration.Player1Nation = player1.GetAttribute("nation");
			}

			var player2 = element["player2"];
			if (player2 != null && player2.HasAttribute("nation"))
			{
				this.Configuration.Player2Nation = player2.GetAttribute("nation");
			}

			var camera = element["camera"];
			if (camera != null)
			{
				if (camera.HasAttribute("speed"))
				{
					this.Configuration.CameraSpeed = float.Parse(camera.GetAttribute("speed"));
				}
			}
		}
		#endregion

		/// <summary>
		/// Inicjalizuje serializer.
		/// </summary>
		/// <param name="cfg">Obiekt do serializacji/deserializacji.</param>
		public ConfigurationSerializer(Interfaces.IConfiguration cfg)
		{
			this.Configuration = cfg;
		}
	}
}
