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
			Gravity = 200f,
			CastleSize = new OpenTK.Vector2(20f, 30f),
			ResourceRenewalTime = 8f,
			ResourceRenewalValue = 30,
			UseFPSCounter = true,
			Player1Nation = "TestNation",
			Player2Nation = "TestNation",
			StartResources = 1000
		};

		/// <summary>
		/// Zasoby, które są w grze.
		/// </summary>
		public static readonly string[] Resources = new string[]
		{
			"wood"
		};

		/// <summary>
		/// Ścieżka do zasobów.
		/// </summary>
		public const string RootDirectory = "Content";

		/// <summary>
		/// Folder z danymi użytkownika.
		/// </summary>
		public const string UserData = "Content/UserData";
		#endregion
	}
}
