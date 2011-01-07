namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontener na kontrolki.
	/// </summary>
	public interface IContainer
	{
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		IGameInfo GameInfo { get; set; }

		/// <summary>
		/// Kamera używana przez kontener.
		/// Może być null.
		/// </summary>
		Graphics.ICamera Camera { get; set; }

		/// <summary>
		/// Główna kontrolka.
		/// </summary>
		IContainerControl Root { get; }

		/// <summary>
		/// Uaktualnia wszystkie kontrolki w kontenerze.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Renderuje wszystkie kontrolki.
		/// </summary>
		void Render();
	}
}
