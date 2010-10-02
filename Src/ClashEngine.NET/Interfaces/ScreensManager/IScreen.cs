using OpenTK.Input;

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
	/// <remarks>
	///	Zdarzenia klawiatury i myszki powinny być obsługiwane w odpowiednich metodach zdarzeń, nie w metodzie Update.
	///	Umożliwi to wysyłanie zdarzeń tylko do aktywnych ekranów i przesyłanie zdarzenia będzie mogło zostać przerwane, czego nie umożliwia metoda Update.
	///	Ekran sam powinien sprawdzać, czy np. naciśnięcie przycisku myszy jest nad nim(dotyczy to ekranów typu popup, fullscreen, jak wiadomo, jest tylko jeden).
	/// </remarks>
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
		bool IsPopup { get; set; }

		/// <summary>
		/// Czy ekran jest pełnoekranowy(zasłania wszystko co pod).
		/// Nie może być true jeśli IsPopup == true;
		/// </summary>
		bool IsFullscreen { get; set; }

		/// <summary>
		/// Aktualny stan ekranu.
		/// </summary>
		ScreenState State { get; }
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
		/// <param name="oldState">Stan sprzed zmiany.</param>
		void StateChanged(ScreenState oldState);

		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji komponentu(dodaniu do managera).
		/// </summary>
		void OnInit();

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji komponentu(usunięcie z managera).
		/// </summary>
		void OnDeinit();

		#region Keyboard
		/// <summary>
		/// Zdarzenie naciśnięcia klawisza.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		bool KeyDown(KeyboardKeyEventArgs e);

		/// <summary>
		/// Zdarzenie zwolnienia klawisza.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		bool KeyUp(KeyboardKeyEventArgs e);
		#endregion

		#region Mouse
		/// <summary>
		/// Zdarzenie naciśnięcia przycisku myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		bool MouseButtonDown(MouseButtonEventArgs e);

		/// <summary>
		/// Zdarzenie zwolnienia przycisku myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zdarzenie zostało obsłużone.</returns>
		bool MouseButtonUp(MouseButtonEventArgs e);

		/// <summary>
		/// Zdarzenie poruszenia myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zostało obsłużonę.</returns>
		bool MouseMove(MouseMoveEventArgs e);

		/// <summary>
		/// Zdarzenie "przekręcenia" kółka myszy.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Czy zostało obsłużonę.</returns>
		bool MouseWheelChanged(MouseWheelEventArgs e);
		#endregion
		#endregion
	}
}
