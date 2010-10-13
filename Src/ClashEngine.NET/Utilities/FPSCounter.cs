using System;

namespace ClashEngine.NET.Utilities
{
	using Interfaces.Utilities;
	using ScreensManager;

	/// <summary>
	/// Licznik FPS.
	/// Można dodać go do managera ekranów - będzie automatycznie aktualizowany i, gdy włączymy, automatycznie rysowany(popup).
	/// </summary>
	/// <remarks>
	/// Rysowanie aktualnie NIE JEST wspierane.
	/// </remarks>
	public class FPSCounter
		: Screen, IFPSCounter
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET.FPS");
		
		#region Private Fields
		/// <summary>
		/// Aktualny licznik FPS.
		/// </summary>
		int FPSCount = 0;

		/// <summary>
		/// Czas od ostatniej aktualizacji.
		/// </summary>
		double FPSUpdateTime = 0.0;

		/// <summary>
		/// Czas od ostatniego logowania.
		/// </summary>
		double LogTime = 0.0;

		/// <summary>
		/// Całkowita liczba klatek
		/// </summary>
		long AllFrames = 0;

		/// <summary>
		/// Całkowity czas uruchomienia.
		/// </summary>
		double AllTime = 0.0;
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
		public bool RenderStatistics { get; set; }

		/// <summary>
		/// Czas pomiędzy logowaniem statystyk. 0 - wyłączone.
		/// </summary>
		public float LogStatistics { get; set; }
		#endregion

		public FPSCounter()
			: base("FPSCounter", Interfaces.ScreensManager.ScreenType.Popup)
		{
			this.CurrentFPS = 0;
			this.MinFPS = int.MaxValue;
			this.MaxFPS = int.MinValue;
			this.AverageFPS = 0.0f;
			this.RenderStatistics = false;
			this.LogStatistics = 0.0f;
		}

		#region Screen members
		public override void Update(double delta)
		{
			this.FPSUpdateTime += delta;
			this.AllTime += delta;
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
				this.AverageFPS = (float)(this.AllFrames / this.AllTime);

				this.FPSCount = 0;
				this.FPSUpdateTime = 0.0;
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
			base.Update(delta);
		}

		public override void Render()
		{
			++this.FPSCount;
			++this.AllFrames;
			//if(this.AllFrames == long.MaxValue) // Zabezpieczenie przed buffer overflowem.
			if (this.RenderStatistics)
			{
				throw new NotSupportedException("FPS statistics rendering is not supported");
				base.Render();
			}
		}
		#endregion
	}
}
