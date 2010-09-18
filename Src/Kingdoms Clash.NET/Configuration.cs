namespace Kingdoms_Clash.NET
{
	/// <summary>
	/// Konfiguracja gry.
	/// </summary>
	public class Configuration
		: Interfaces.IConfiguration
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Singleton
		private static Interfaces.IConfiguration Instance_;

		public static Interfaces.IConfiguration Instance
		{
			get
			{
				if (Instance_ == null)
				{
					Instance_ = new Configuration();
				}
				return Instance_;
			}
		}
		#endregion

		#region IConfiguration Members
		/// <summary>
		/// Szerokość okna.
		/// </summary>
		public int WindowWidth { get; set; }

		/// <summary>
		/// Wysokość okna.
		/// </summary>
		public int WindowHeight { get; set; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		public bool Fullscreen { get; set; }

		/// <summary>
		/// Określa ile pikseli przypada na jedną jednostkę gry.
		/// Aktualnie zahardcodowane na stałą wartość.
		/// TODO: dopisać obliczanie.
		/// </summary>
		public float PixelsWidthPerUnit
		{
			get { return 10; }
		}

		/// <summary>
		/// Określa ile pikseli przypada na jedną jednostkę gry.
		/// Aktualnie zahardcodowane na stałą wartość.
		/// TODO: dopisać obliczanie.
		/// </summary>
		public float PixelsHeightPerUnit
		{
			get { return 10; }
		}
		#endregion

		private Configuration()
		{
			Logger.Trace("Loading configuration");
			this.WindowWidth = 800;
			this.WindowHeight = 600;
			this.Fullscreen = false;
		}
	}
}
