using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Kingdoms_Clash.NET.UserData
{
	/// <summary>
	/// Loader dla danych użytkownika.
	/// </summary>
	public class Loader
		: Interfaces.IUserDataLoader
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region IUserDataLoader Members
		/// <summary>
		/// Ścieżka do głównego folderu z danymi użytkownika.
		/// </summary>
		public string Path { get; private set; }

		/// <summary>
		/// Ścieżka do pliku konfiguracyjnego.
		/// </summary>
		public string ConfigurationFile { get; private set; }

		/// <summary>
		/// Lista komponentów użytkownika.
		/// </summary>
		public IList<Type> Components { get; private set; }

		/// <summary>
		/// Załadowane nacje.
		/// Dostępne dopiero po wywołaniu LoadNations.
		/// </summary>
		public IList<Interfaces.Units.INation> Nations { get; private set; }

		/// <summary>
		/// Ładuje nacje z folderu Path/Nations.
		/// </summary>
		/// <returns>Lista nacji.</returns>
		public void LoadNations()
		{
			do
			{
				Logger.Info("Loading nations");
				string pathToNations = System.IO.Path.Combine(this.Path, "Nations");
				pathToNations = System.IO.Path.GetFullPath(pathToNations);
				if (!System.IO.Directory.Exists(pathToNations))
				{
					Logger.Error("\tDirectory of nations '{0}' does not exist, cannot load nations", pathToNations);
					break;
				}

				string[] files = null;
				try
				{
					files = Directory.GetFiles(pathToNations);
				}
				catch (Exception ex)
				{
					Logger.ErrorException("\tCannot list files in directory", ex);
					break;
				}
				if (files.Length == 0)
				{
					Logger.Warn("\tThere is no nation to load");
					break;
				}

				foreach (var file in files)
				{
					var nation = new NationLoader(file, this.Components).Create();
					if (nation != null)
					{
						Logger.Info("\tNation {0} loaded", nation.Name);
						this.Nations.Add(nation);
					}
				}
				
			} while (false);
		}

		/// <summary>
		/// Ładuje konfigurację.
		/// </summary>
		public void LoadConfiguration()
		{
			try
			{
				XmlDocument xml = new XmlDocument();
				xml.Load(System.IO.Path.GetFullPath(this.ConfigurationFile));

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

		/// <summary>
		/// Inicjalizuje loader.
		/// </summary>
		/// <param name="rootPath">Główna ścieżka do danych.</param>
		/// <param name="configFile">Ścieżka do pliku konfiguracyjnego.</param>
		public Loader(string rootPath, string configFile)
		{
			this.Path = rootPath;
			this.ConfigurationFile = configFile;
			this.Components = new List<Type>();
			this.Nations = new List<Interfaces.Units.INation>();
		}
	}
}
