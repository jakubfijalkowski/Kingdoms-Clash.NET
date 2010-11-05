using System.Drawing;

namespace ClashEngine.NET.Graphics.Gui.Base
{
	using Utilities;

	/// <summary>
	/// Kontrolka GUI, którą można zamknąć w prosokącie.
	/// </summary>
	public abstract class RectangleGuiControl
		: GuiControl
	{
		#region Properties
		/// <summary>
		/// Prostokąt, w którym kontrolka się mieści.
		/// </summary>
		public RectangleF Rectangle { get; private set; }
		#endregion

		#region IGuiControl Members
		/// <summary>
		/// Sprawdza, czy myszka znajduje się nad kontrolką.
		/// </summary>
		/// <returns>Prawda, gdy myszka jest nad kontrolką. W przeciwnym razie fałsz.</returns>
		public override bool ContainsMouse()
		{
			return this.Rectangle.Contains(this.Data.Input.TransformedMousePosition);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="rectangle">Prostokąt, w którym się mieści kontrolka. Zobacz <see cref="Rectangle"/>.</param>
		public RectangleGuiControl(string id, RectangleF rectangle)
			: base(id)
		{
			this.Rectangle = rectangle;
		}
		#endregion
	}
}
