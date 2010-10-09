using System;
using System.Collections.Generic;
using System.IO;

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
		public IList<Interfaces.Units.INation> LoadNations()
		{
			List<Interfaces.Units.INation> nations = new List<Interfaces.Units.INation>();

			do
			{
				string pathToNations = System.IO.Path.Combine(this.Path, "Nations");
				pathToNations = System.IO.Path.GetFullPath(pathToNations);
				if (!System.IO.Directory.Exists(pathToNations))
				{
					Logger.Error("Directory od nations '{0}' does not exist, cannot load nations", pathToNations);
					break;
				}

				string[] files = null;
				try
				{
					files = Directory.GetFiles(pathToNations);
				}
				catch (Exception ex)
				{
					Logger.ErrorException("Cannot list files in directory", ex);
					break;
				}
				if (files.Length == 0)
				{
					Logger.Warn("There is no nation to load");
					break;
				}

				foreach (var file in files)
				{
					var nation = new NationLoader(file, this.Components).Create();
					if (nation != null)
					{
						nations.Add(nation);
					}
				}
				
			} while (false);

			return nations;
		}
		#endregion

		/// <summary>
		/// Inicjalizuje loader.
		/// </summary>
		/// <param name="rootPath">Główna ścieżka do danych.</param>
		public Loader(string rootPath)
		{
			this.Path = rootPath;
			this.Components = new List<Type>();
		}
	}
}
