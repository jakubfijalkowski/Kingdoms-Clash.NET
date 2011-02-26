
namespace ClashEngine.NET.Utilities
{
	using Extensions;
	using Interfaces;
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Resources;
	using Interfaces.Utilities;

	/// <summary>
	/// Licznik FPS.
	/// Można dodać go do managera ekranów - będzie automatycznie aktualizowany i, gdy włączymy, automatycznie rysowany(popup).
	/// </summary>
	public class FPSCounter
		: IFPSCounter
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("FPS");
		
		#region Private Fields
		/// <summary>
		/// Aktualny licznik FPS.
		/// </summary>
		private int FPSCount = 0;

		/// <summary>
		/// Czas od ostatniej aktualizacji.
		/// </summary>
		private double FPSUpdateTime = 0.0;

		/// <summary>
		/// Czas od ostatniego logowania.
		/// </summary>
		private double LogTime = 0.0;

		/// <summary>
		/// Liczba klatek do liczenia średniej.
		/// </summary>
		private short AvgFrames = 0;

		/// <summary>
		/// Czas do liczenia średniej.
		/// </summary>
		private double AvgTime = 0.0;

		/// <summary>
		/// Gui.
		/// Używany tylko, jeśli RenderStatistics == true.
		/// </summary>
		private IContainer Gui = null;

		/// <summary>
		/// Tekst z liczbą FPS.
		/// Używana tylko, jeśli RenderStatistics == true.
		/// </summary>
		private Interfaces.Graphics.Gui.Objects.IText Text = null;

		/// <summary>
		/// Kamera używana przez ekran.
		/// </summary>
		private Interfaces.Graphics.ICamera Camera;
		#endregion
		
		#region IFPSCounter Members
		/// <summary>
		/// Aktualny licznik FPS.
		/// </summary>
		public float CurrentFPS { get; private set; }

		/// <summary>
		/// Minimalna wartość FPS.
		/// </summary>
		public float MinFPS { get; private set; }

		/// <summary>
		/// Maksymalna wartość FPS.
		/// </summary>
		public float MaxFPS { get; private set; }

		/// <summary>
		/// Średnia wartość FPS.
		/// </summary>
		public float AverageFPS { get; private set; }

		/// <summary>
		/// Czy odrysowywać aktualny licznik FPS na ekranie.
		/// </summary>
		public bool RenderStatistics { get; private set; }

		/// <summary>
		/// Czas pomiędzy logowaniem statystyk. 0 - wyłączone.
		/// </summary>
		public float LogStatistics { get; set; }
		#endregion

		#region IScreen Members
		public string Id { get { return "FPSCounter"; }	}
		public Interfaces.IScreensManager OwnerManager { get; set; }
		public IGameInfo GameInfo { get; set; }
		public Interfaces.ScreenType Type { get { return Interfaces.ScreenType.Popup; } }
		public Interfaces.ScreenState State { get; set; }

		public void OnInit()
		{
			if (this.RenderStatistics)
			{
				this.Gui = new ClashEngine.NET.Graphics.Gui.Container(this.GameInfo);

				var pane = new Graphics.Gui.Controls.Pane();
				pane.Id = "FPSPanel";
				pane.Position = new OpenTK.Vector2(0, 0);
				pane.Size = new OpenTK.Vector2(100, 30);
				pane.Objects.Add(this.Text);
				this.Gui.Root.Controls.Add(pane);
			}
		}

		public void OnDeinit()
		{ }

		public void Update(double delta)
		{
			this.FPSUpdateTime += delta;
			this.AvgTime += delta;
			if (this.FPSUpdateTime > 1.0)
			{
				float fps = (float)(this.FPSCount / this.FPSUpdateTime);
				this.CurrentFPS = fps;
				if (this.MinFPS > fps)
				{
					this.MinFPS = fps;
				}
				if (this.MaxFPS < fps)
				{
					this.MaxFPS = fps;
				}

				this.FPSCount = 0;
				this.FPSUpdateTime = 0.0;
			}
			if (this.AvgFrames >= 50)
			{
				float currAverage = (float)(this.AvgFrames / this.AvgTime);

				if (this.RenderStatistics && (int)currAverage != (int)this.AverageFPS)
				{
					this.Text.TextValue = "FPS: " + (int)currAverage;
				}
				this.AverageFPS = currAverage;
				this.AvgFrames = 0;
				this.AvgTime = 0f;
			}
			if (this.LogStatistics > 0.0f)
			{
				this.LogTime += delta;
				if (this.LogTime > this.LogStatistics)
				{
					this.LogTime -= this.LogStatistics;
					Logger.Info("Current FPS: {0}, max FPS: {1}, min FPS: {2}, average FPS: {3}", this.CurrentFPS, this.MaxFPS, this.MinFPS, this.AverageFPS);
				}
			}
		}

		public void Render()
		{
			++this.FPSCount;
			++this.AvgFrames;
			if (this.RenderStatistics)
			{
				this.GameInfo.Renderer.Camera = this.Camera;
				this.Gui.Render();
				this.GameInfo.Renderer.Flush();
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje licznik tak, by FPS był tylko logowany.
		/// </summary>
		/// <param name="logStatistics">Odstęp pomiędzy logowaniami.</param>
		public FPSCounter(float logStatistics)
		{
			this.CurrentFPS = 0;
			this.MinFPS = int.MaxValue;
			this.MaxFPS = int.MinValue;
			this.AverageFPS = 0.0f;
			this.RenderStatistics = false;
			this.LogStatistics = logStatistics;
		}

		/// <summary>
		/// Inicjalizuje licznik tak, by tylko wyświetlał FPS.
		/// </summary>
		/// <param name="font">Czcionka użyta do wyświetlania.</param>
		/// <param name="textColor">Kolor.</param>
		/// <param name="screenSize">Rozmiar ekranu.</param>
		public FPSCounter(IFont font, System.Drawing.Color textColor, OpenTK.Vector2 screenSize)
			: this(0)
		{
			this.RenderStatistics = true;
			this.Camera = new Graphics.Cameras.Movable2DCamera(screenSize, new System.Drawing.RectangleF(0, 0, screenSize.X, screenSize.Y));

			this.Text = new Graphics.Gui.Objects.Text();
			this.Text.Font = font;
			this.Text.Color = textColor.ToVector4();
		}
		#endregion
	}
}
