namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontener na kontrolki.
	/// W kontenerze nie mogą istnieć dwie kontrolki o takim samym Id.
	/// </summary>
	public interface IContainer
	{
		/// <summary>
		/// Wejście dla GUI.
		/// </summary>
		IInput Input { get; set; }

		/// <summary>
		/// Renderer GUI.
		/// </summary>
		Graphics.IRenderer Renderer { get; set; }

		/// <summary>
		/// Kamera używana przez kontener.
		/// Może być null.
		/// </summary>
		Graphics.ICamera Camera { get; set; }

		/// <summary>
		/// Kolekcja kontrolek.
		/// </summary>
		IControlsCollection Controls { get; }

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
	}
}
