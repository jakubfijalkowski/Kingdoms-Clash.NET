using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontener na kontrolki.
	/// W kontenerze nie mogą istnieć dwie kontrolki o takim samym Id.
	/// </summary>
	public interface IGuiContainer
		: ICollection<IGuiControl>
	{
		/// <summary>
		/// Wejście dla GUI.
		/// </summary>
		IInput Input { get; set; }

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Kontrolka lub null, gdy nie znaleziono.</returns>
		IGuiControl this[string id] { get; }

		/// <summary>
		/// Uaktualnia wszystkie kontrolki w kontenerze.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Renderuje wszystkie kontrolki.
		/// </summary>
		void Render();

		/// <summary>
		/// Sprawdza stan kontrolki za pomocą <see cref="IGuiControl.Check"/>.
		/// </summary>
		/// <param name="id">Identyfikator kontrolki.</param>
		/// <returns>Nr akcji bądź 0, gdy żadna akcja nie zaszła.</returns>
		int Control(string id);

		/// <summary>
		/// Usuwa kontrolkę o wskazanym ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Czy udało się usunąć kontrolkę.</returns>
		bool Remove(string id);
	}
}
