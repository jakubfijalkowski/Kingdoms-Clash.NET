using System;
using System.Xml.Serialization;

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
