namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Stan ekranu.
	/// </summary>
	public enum ScreenState
	{
		/// <summary>
		/// Aktywowany.
		/// </summary>
		Activated = 0,

		/// <summary>
		/// Przykryty przez inne ekrany(ale nadal rysowany).
		/// </summary>
		Covered,

		/// <summary>
		/// Ukryty(całkowicie).
		/// </summary>
		Hidden,

		/// <summary>
		/// Deaktywowany.
		/// </summary>
		Deactivated
	}

	/// <summary>
	/// Typ ekranu.
	/// </summary>
	public enum ScreenType
	{
		/// <summary>
		/// Normalny.
		/// Gdy aktywny blokuje ekrany "za" przed aktualizacją.
		/// </summary>
		Normal,

		/// <summary>
		/// Pełnoekranowy.
		/// Gdy aktywny blokuje ekrany "za" przed aktualizacją i rysowaniem.
		/// </summary>
		Fullscreen,

		/// <summary>
		/// Wyskakujące "okienko".
		/// Nie oddziaływuje na ekrany "za".
		/// </summary>
		Popup
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
		/// Identyfikator ekranu.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Manager - właściciel.
		/// Ustawiane przez niego samego.
		/// </summary>
		IScreensManager OwnerManager { get; set; }

		/// <summary>
		/// Wejście.
		/// Ustawiane przez właściciela.
		/// </summary>
		IInput Input { get; set; }

		/// <summary>
		/// Manager zasobów dla ekranu.
		/// Ustawiany przez właściciela, ale nie ma wymogu, by to właśnie jego używać.
		/// </summary>
		IResourcesManager Content { get; set; }

		/// <summary>
		/// Renderer.
		/// Ustawiany przez właściciela, ale nie ma wymogu, by go używać.
		/// </summary>
		Graphics.IRenderer Renderer { get; set; }

		/// <summary>
		/// Typ ekranu.
		/// </summary>
		ScreenType Type { get; }

		/// <summary>
		/// Aktualny stan ekranu.
		/// </summary>
		/// <remarks>
		/// Zmieniać tylko przez manager ekranów.
		/// </remarks>
		ScreenState State { get; set; }
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji komponentu(dodaniu do managera).
		/// </summary>
		void OnInit();

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji komponentu(usunięcie z managera).
		/// </summary>
		void OnDeinit();

		/// <summary>
		/// Uaktualnienie.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void Update(double delta);

		/// <summary>
		/// Ekran ma się odrysować.
		/// </summary>
		void Render();
		#endregion
	}
}
