using System.Linq;
using System.Reflection;

namespace Kingdoms_Clash.NET
{
	/// <summary>
	/// Klasa z domyślnymi wartościami.
	/// </summary>
	static class Defaults
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Basic
		/// <summary>
		/// Domyślna konfiguracja.
		/// </summary>
		public static readonly Interfaces.IConfiguration DefaultConfiguration = new Configuration()
		{
			WindowSize = new System.Drawing.Size(800, 600),
			Fullscreen = false,
			ScreenSize = new OpenTK.Vector2(100f, 75f),
			VSync = false,
			CameraSpeed = 100f,
			MapMargin = 75f / 2f,
			Gravity = 100f,
			CastleSize = new OpenTK.Vector2(20f, 30f),
			ResourceRenewalTime = 8f,
			ResourceRenewalValue = 30,
			UseFPSCounter = true,
			Player1Nation = "TestNation",
			Player2Nation = "TestNation",
			StartResources = 1000
		};

		/// <summary>
		/// Ścieżka do zasobów.
		/// </summary>
		public const string ContentDirectory = "Content";

		/// <summary>
		/// Folder z danymi użytkownika.
		/// </summary>
		public const string UserData = "Content/UserData";

		/// <summary>
		/// Ścieżka do pliku konfiguracyjnego.
		/// </summary>
		public const string ConfigurationFile = "Content/Configuration.xml";
		#endregion

		#region Resources
		/// <summary>
		/// Lista zasobów dostępnych w grze.
		/// </summary>
		/// TODO: dodać zlokalizowane teksty
		public static readonly Interfaces.Resources.IResourceDescription[] Resources = new Interfaces.Resources.IResourceDescription[]
		{
			new Resources.ResourceDescription("wood", "Drewno", "Podstawowy zasób gry", new OpenTK.Vector2(5f, 10f), "Resources/Wood.png")
		};
		#endregion
	}
}
