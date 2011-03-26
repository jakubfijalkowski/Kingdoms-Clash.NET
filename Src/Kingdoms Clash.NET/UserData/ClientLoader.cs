using System.Xml;

namespace Kingdoms_Clash.NET.UserData
{
	/// <summary>
	/// Loader danych dla klienta gry.
	/// </summary>
	internal class ClientLoader
		: LoaderBase
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		
		#region LoaderBase Members
		/// <summary>
		/// Ładuje konfiguracje.
		/// </summary>
		public override void LoadConfiguration()
		{
			try
			{
				XmlDocument xml = new XmlDocument();
				xml.Load(System.IO.Path.GetFullPath(System.IO.Path.Combine(this.RootPath, "Configuration.xml")));

				var cfg = xml["configuration"];
				if (cfg == null)
				{
					throw new XmlException("Cannot find 'configuration' element");
				}
				new ConfigurationSerializer(Configuration.Instance).Deserialize(cfg);
				Logger.Info("Configuration loaded");
			}
			catch (System.Exception ex)
			{
				Logger.WarnException("Cannot load configuration", ex);
				Configuration.UseDefault();
			}
		} 
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje loader.
		/// </summary>
		/// <param name="rootPath">Główna ścieżka do danych.</param>
		/// <param name="userDataPath">Ścieżka do danych użytkownika.</param>
		public ClientLoader(string rootPath, string userDataPath)
			: base(rootPath, userDataPath)
		{ }
		#endregion
	}
}
