using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xaml;

namespace Kingdoms_Clash.NET.UserData
{
	using Interfaces.Resources;
	using Interfaces.Units;
	using Resources;

	/// <summary>
	/// Loader dla danych użytkownika.
	/// </summary>
	public abstract class LoaderBase
		: Interfaces.IUserDataLoader
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region IUserDataLoader Members
		/// <summary>
		/// Ścieżka do głównego folderu z danymi użytkownika.
		/// </summary>
		public string RootPath { get; private set; }

		/// <summary>
		/// Ścieżka do głównego folderu z danymi użytkownika.
		/// </summary>
		public string UserDataPath { get; private set; }

		/// <summary>
		/// Załadowane nacje.
		/// Dostępne dopiero po wywołaniu LoadNations.
		/// </summary>
		public IList<Interfaces.Units.INation> Nations { get; private set; }

		/// <summary>
		/// Sumy kontrolne nacji.
		/// </summary>
		public IList<byte[]> NationsCheckSums { get; private set; }

		/// <summary>
		/// Ładuje nacje z folderu Path/Nations.
		/// </summary>
		/// <returns>Lista nacji.</returns>
		public void LoadNations()
		{
			do
			{
				Logger.Info("Loading nations");
				string pathToNations = System.IO.Path.Combine(this.UserDataPath, "Nations");
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
					try
					{
						var nation = XamlServices.Load(file) as INation;
						using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
						using (MD5 md5 = new MD5CryptoServiceProvider())
						{
							var hash = md5.ComputeHash(stream);
							this.NationsCheckSums.Add(hash);
						}

						Logger.Info("\tNation {0} loaded", nation.Name);
						this.Nations.Add(nation);
					}
					catch (Exception ex)
					{
						Logger.WarnException("Cannot load nation from file " + file, ex);
					}
				}
			} while (false);
		}

		/// <summary>
		/// Ładuje konfigurację.
		/// </summary>
		public abstract void LoadConfiguration();

		/// <summary>
		/// Ładuje zasoby.
		/// </summary>
		public void LoadResources()
		{
			List<IResourceDescription> resources = new List<IResourceDescription>();
			do
			{
				Logger.Info("Loading resources");
				string pathToResources = System.IO.Path.Combine(this.RootPath, "Resources");
				pathToResources = System.IO.Path.GetFullPath(pathToResources);
				if (!System.IO.Directory.Exists(pathToResources))
				{
					Logger.Error("\tDirectory of resources '{0}' does not exist, cannot load resources", pathToResources);
					break;
				}

				string[] files = null;
				try
				{
					files = Directory.GetFiles(pathToResources, "*.xml");
				}
				catch (Exception ex)
				{
					Logger.ErrorException("\tCannot list files in directory", ex);
					break;
				}
				if (files.Length == 0)
				{
					Logger.Warn("\tThere is no resources to load");
					break;
				}

				foreach (var file in files)
				{
					try
					{
						resources.Add(ResourceSerializer.Deserialize(file));
					}
					catch (Exception ex)
					{
						Logger.WarnException("Cannot load resource from file " + file, ex);
					}
				}
			} while (false);

			List<IResourceDescription> outputResources = new List<IResourceDescription>();
			foreach (var resource in Defaults.Resources)
			{
				var res = resources.Find(r => r.Id == resource);
				if (res == null)
				{
					Logger.Error("Cannot find description of resource '{0}'", resource);
				}
				else
				{
					outputResources.Add(res);
					Logger.Info("\tResource {0} loaded", resource);
				}
			}
			if (outputResources.Count != Defaults.Resources.Length)
			{
				Logger.Error("There is not enough resources description");
			}
			ResourcesList.Instance.Init(outputResources);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje loader.
		/// </summary>
		/// <param name="rootPath">Główna ścieżka do danych.</param>
		/// <param name="userDataPath">Ścieżka do danych użytkownika.</param>
		public LoaderBase(string rootPath, string userDataPath)
		{
			this.RootPath = rootPath;
			this.UserDataPath = userDataPath;
			this.Nations = new List<Interfaces.Units.INation>();
			this.NationsCheckSums = new List<byte[]>();
		}
		#endregion
	}
}
