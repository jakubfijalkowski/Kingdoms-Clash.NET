using System.Drawing;

namespace ClashEngine.NET.Gui.Base
{
	/// <summary>
	/// Bazowa klasa dla kontrolki GUI, która potrzebuje wiedzieć, czy w poprzedniej klatce była aktywna.
	/// </summary>
	public abstract class GuiControlWithWasActive
		: GuiControl
	{
		#region Private fields
		/// <summary>
		/// Czy kontrolka jest aktywna w aktualnej klatce.
		/// </summary>
		private bool IsActive = false;
		#endregion

		#region Properties
		/// <summary>
		/// Czy była aktywna.
		/// </summary>
		protected bool WasActive { get; private set; }
		#endregion

		#region IGuiControl Members
		/// <summary>
		/// Sprawdza, czy kontrolka była aktywna w poprzedniej klatce.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.WasActive = this.IsActive;
			this.IsActive = this.Data.Active == this;
		}
		#endregion

		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public GuiControlWithWasActive(string id)
			: base(id)
		{ }
	}

	/// <summary>
	/// Bazowa klasa dla kontrolki GUI, która potrzebuje wiedzieć, czy w poprzedniej klatce była aktywna.
	/// </summary>
	public abstract class RectangleGuiControlWithWasActive
		: RectangleGuiControl
	{
		#region Private fields
		/// <summary>
		/// Czy kontrolka jest aktywna w aktualnej klatce.
		/// </summary>
		private bool IsActive = false;
		#endregion

		#region Properties
		/// <summary>
		/// Czy była aktywna.
		/// </summary>
		protected bool WasActive { get; private set; }
		#endregion

		#region IGuiControl Members
		/// <summary>
		/// Sprawdza, czy kontrolka była aktywna w poprzedniej klatce.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.WasActive = this.IsActive;
			this.IsActive = this.Data.Active == this;
		}
		#endregion

		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="rectangle">Prostoką, w którym mieści się kontrolka. Zobacze <see cref="RectangleGuiControl.Rectangle"/>.</param>
		public RectangleGuiControlWithWasActive(string id, RectangleF rectangle)
			: base(id, rectangle)
		{ }
	}
}
