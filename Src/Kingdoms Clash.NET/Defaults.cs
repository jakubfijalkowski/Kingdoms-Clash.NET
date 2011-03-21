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
#if !SERVER
		/// <summary>
		/// Domyślna konfiguracja.
		/// </summary>
		public static readonly Interfaces.IConfiguration DefaultClientConfiguration = new Configuration()
		{
			WindowSize = new System.Drawing.Size(800, 600),
			Fullscreen = false,
			VSync = false,
			CameraSpeed = 100f,
			UseFPSCounter = true,
			Player1Nation = "TestNation",
			Player2Nation = "TestNation"
		};
#endif

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
