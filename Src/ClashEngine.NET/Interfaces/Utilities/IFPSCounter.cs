namespace ClashEngine.NET.Interfaces.Utilities
{
	/// <summary>
	/// Interfejs dla licznika FPS.
	/// </summary>
	public interface IFPSCounter
		: IScreen
	{
		/// <summary>
		/// Aktualny licznik FPS.
		/// </summary>
		float CurrentFPS { get; }

		/// <summary>
		/// Minimalna wartość FPS.
		/// </summary>
		float MinFPS { get; }

		/// <summary>
		/// Maksymalna wartość FPS.
		/// </summary>
		float MaxFPS { get; }

		/// <summary>
		/// Średnia wartość FPS.
		/// </summary>
		float AverageFPS {get;}

		/// <summary>
		/// Czy odrysowywać aktualny licznik FPS na ekranie.
		/// </summary>
		bool RenderStatistics { get; }

		/// <summary>
		/// Czas pomiędzy logowaniem statystyk. 0 - wyłączone.
		/// </summary>
		float LogStatistics { get; set; }
	}
}
