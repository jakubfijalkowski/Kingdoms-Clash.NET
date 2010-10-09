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
			CastleSize = new OpenTK.Vector2(20f, 30f)
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
	
		/// <summary>
		/// Rejestruje wbudowane komponenty do loadera.
		/// </summary>
		/// <param name="loader"></param>
		public static void RegisterBuiltInComponents(Interfaces.IUserDataLoader loader)
		{
			var current = Assembly.GetExecutingAssembly();

			Logger.Trace("Loading built-in components");
			foreach (var type in current.GetTypes())
			{
				if (type.Namespace == "Kingdoms_Clash.NET.Units.Components" &&
					type.GetInterfaces().Any(i => i == typeof(Interfaces.Units.IUnitComponentDescription)))
				{
					Logger.Trace("\tComponent {0} loaded", type.Name);
					loader.Components.Add(type);
				}
			}
		}
	}
}
