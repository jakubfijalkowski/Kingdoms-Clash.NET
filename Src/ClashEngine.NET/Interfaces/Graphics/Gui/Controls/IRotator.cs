namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
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
		IRotatorObjectsCollection Items { get; }

		/// <summary>
		/// Maksymalna liczba elementów, które mogą być aktualnie wybrane.
		/// </summary>
		uint MaxSelectedItems { get; set; }

		/// <summary>
		/// Pierwszy wybrany element.
		/// </summary>
		int First { get; set; }

		/// <summary>
		/// Pobiera jeden z wybranych elementów.
		/// </summary>
		/// <remarks>Elementy mogą być puste.</remarks>
		/// <param name="index">Indeks. Od 0 do SelectedItemsCount.</param>
		/// <returns></returns>
		object this[int index] { get; }

		/// <summary>
		/// Aktualnie wybrane elementy.
		/// </summary>
		IRotatorSelectedItems Selected { get; }
	}
}
