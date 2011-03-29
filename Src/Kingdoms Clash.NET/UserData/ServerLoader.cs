using System;
using System.Linq;
using System.Xml;

namespace Kingdoms_Clash.NET.Server.UserData
{
	using NET.UserData;

	/// <summary>
	/// Loader danych dla serwera.
	/// </summary>
	internal class ServerLoader
		: LoaderBase
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET.Server");

		#region LoaderBase Members
		/// <summary>
		/// Ładuje konfiguracje.
		/// </summary>
		public override void LoadConfiguration()
		{
			XmlDocument doc = new XmlDocument();
			try
			{
				int tmp = 0;

				var cfg = doc.DocumentElement;
				if (cfg.Name != "serverConfiguration")
					throw new XmlException("Invalid root element. Must be serverConfiguration");

				#region Port
				var portStr = cfg.GetAttribute("port");
				if (string.IsNullOrWhiteSpace(portStr))
					throw new XmlException("Attribute 'port' must be specified");

				if (!int.TryParse(portStr, out tmp))
					throw new Exception("Cannot parse 'port' value");
				if (tmp < System.Net.IPEndPoint.MaxPort || tmp > System.Net.IPEndPoint.MaxPort)
					throw new Exception(string.Format("Port must be from range {0}..{1}", System.Net.IPEndPoint.MinPort, System.Net.IPEndPoint.MaxPort));
				ServerConfiguration.Instance.Port = tmp;
				#endregion

				#region MaxSpectators
				var maxSpectators = cfg.GetAttribute("maxSpectators");
				int.TryParse(maxSpectators, out tmp);
				ServerConfiguration.Instance.MaxSpectators = (uint)(tmp > 0 ? tmp : 0);
				#endregion

				#region Name
				var nameElement = cfg["name"];
				if (nameElement == null || string.IsNullOrWhiteSpace(nameElement.Value))
					throw new XmlException("Element 'name' must be specified");
				ServerConfiguration.Instance.Name = nameElement.Value; 
				#endregion

				#region Controller
				var controllerElement = cfg["controller"];
				if (controllerElement == null || string.IsNullOrWhiteSpace(controllerElement.Value))
					throw new XmlException("Element 'controller' must be specified");
				ServerConfiguration.Instance.GameController = Type.GetType(controllerElement.Value);
				if (ServerConfiguration.Instance.GameController == null)
					throw new Exception("Cannot find controller");
				if(!ServerConfiguration.Instance.GameController.GetInterfaces().Any(t => t == typeof(NET.Interfaces.Controllers.IGameController)))
					throw new Exception("Controller must derive from IGameController interface");
				#endregion

				#region VictoryRules
				var rulesElement = cfg["rules"];
				if (controllerElement == null || string.IsNullOrWhiteSpace(controllerElement.Value))
					throw new XmlException("Element 'rules' must be specified");
				ServerConfiguration.Instance.VictoryRules = Type.GetType(controllerElement.Value);
				if (ServerConfiguration.Instance.VictoryRules == null)
					throw new Exception("Cannot find rules");
				if(!ServerConfiguration.Instance.GameController.GetInterfaces().Any(t => t == typeof(NET.Interfaces.Controllers.Victory.IVictoryRules)))
					throw new Exception("Controller must derive from IVictoryRules interface");
				#endregion
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Cannot load configuration", ex);
				throw;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje loader.
		/// </summary>
		/// <param name="rootPath">Główna ścieżka do danych.</param>
		/// <param name="userDataPath">Ścieżka do danych użytkownika.</param>
		public ServerLoader(string rootPath, string userDataPath)
			: base(rootPath, userDataPath)
		{ }
		#endregion
	}
}
