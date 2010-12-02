namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls.Controls
{
	/// <summary>
	/// Kontrolka - rotator.
	/// </summary>
	public interface IRotator
		: IControl
	{
		/// <summary>
		/// Obiekty rotatora.
		/// </summary>
		IRotatorObjectsCollection Objects { get; }

		/// <summary>
		/// Liczba elementów które mogą być aktualnie wyświetlane.
		/// </summary>
		uint SelectedItemsCount { get; set; }

		/// <summary>
		/// Rozmiar pojedynczej strony.
		/// </summary>
		uint PageSize { get; set; }

		/// <summary>
		/// Aktualna strona.
		/// </summary>
		uint CurrentPage { get; set; }

		/// <summary>
		/// Pobiera jeden z wybranych elementów.
		/// </summary>
		/// <param name="index">Indeks. Od 0 do SelectedItemsCount.</param>
		/// <returns></returns>
		object this[int index] { get; }
	}
}
