namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Dane UI.
	/// </summary>
	public interface IUIData
	{
		/// <summary>
		/// Aktualnie "gorąca" kontrolka.
		/// 
		/// Np. użytkownik trzyma nad nią myszkę, ale jeszcze nie kliknął.
		/// </summary>
		IControl Hot { get; set; }

		/// <summary>
		/// Aktualnie aktywna kontrolka.
		/// 
		/// Aktualnie katywna kontrolka - np. button zaraz przed wciśnięciem.
		/// </summary>
		IControl Active { get; set; }

		/// <summary>
		/// Wejście dla GUI.
		/// </summary>
		IInput Input { get; }

		/// <summary>
		/// Renderer.
		/// </summary>
		IRenderer Renderer { get; }
	}
}
