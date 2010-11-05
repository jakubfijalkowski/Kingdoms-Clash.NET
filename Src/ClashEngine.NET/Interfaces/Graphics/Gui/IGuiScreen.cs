namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Ekran jako kontener na kontrolki.
	/// </summary>
	public interface IGuiScreen
		: IGuiContainer, IScreen
	{
		/// <summary>
		/// Prostokąt, w którym zawiera się GUI.
		/// Nadpisuje IInput.MouseTransformation i jest używane przez kamerę.
		/// </summary>
		System.Drawing.RectangleF Rectangle { get; }
	}
}
