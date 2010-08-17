namespace ClashEngine.NET.Interfaces.ScreensManager
{
	/// <summary>
	/// Stan ekranu.
	/// </summary>
	public enum ScreenState
	{
		/// <summary>
		/// Aktywny.
		/// </summary>
		Active,

		/// <summary>
		/// Nieaktywny(np. zasłonięty przez inny nie-popup.
		/// </summary>
		Inactive,

		/// <summary>
		/// Zamknięty(nieodrysowywany ani nieuaktualniany, ale pozostający w managerze).
		/// </summary>
		Closed
	}

	/// <summary>
	/// Interfejs ekranu.
	/// </summary>
	public interface IScreen
	{
		#region Properties
		/// <summary>
		/// Manager - rodzic.
		/// </summary>
		IScreensManager Manager { get; }

		/// <summary>
		/// Czy ekran jest ekranem "wyskakującym", czyli nie zasłania reszty a co za tym idzie pozwala na ich aktualizację.
		/// Nie może być true jeśli IsFullscreen == true.
		/// </summary>
		bool IsPopup
		{
			get;
			set;
		}

		/// <summary>
		/// Czy ekran jest pełnoekranowy(zasłania wszystko co pod).
		/// Nie może być true jeśli IsPopup == true;
		/// </summary>
		bool IsFullscreen
		{
			get;
			set;
		}

		/// <summary>
		/// Aktualny stan ekranu.
		/// </summary>
		ScreenState State
		{
			get;
		}
		#endregion

		#region Object management
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="screensManager">Manager ekranów.</param>
		void Init(IScreensManager screensManager);

		/// <summary>
		/// Zmienia stan ekranu.
		/// Powinno wywoływać StateChanged.
		/// </summary>
		/// <param name="state">Nowy stan.</param>
		void ChangeState(ScreenState state);
		#endregion

		#region Events
		/// <summary>
		/// Uaktualnienie.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void Update(double delta);

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		void Render();

		/// <summary>
		/// Zdarzenie zmiany stanu ekranu.
		/// </summary>
		void StateChanged();
		#endregion
	}
}
