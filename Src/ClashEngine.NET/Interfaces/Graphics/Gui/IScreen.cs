namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Ekran jako kontener na kontrolki.
	/// </summary>
	public interface IScreen
		: Interfaces.IScreen
	{
		/// <summary>
		/// Prostokąt, w którym zawiera się GUI.
		/// </summary>
		System.Drawing.RectangleF Rectangle { get; }

		/// <summary>
		/// Kontener GUI.
		/// </summary>
		IContainer Gui { get; }
	}
}
