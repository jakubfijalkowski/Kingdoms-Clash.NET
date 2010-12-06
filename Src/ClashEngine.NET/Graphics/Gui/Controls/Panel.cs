namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Panel - nie uczestniczy w interakcji z użytkownikiem, jest "statyczny".
	/// </summary>
	public class Panel
		: ControlBase, IPanel
	{
		#region IContainer Members
		/// <summary>
		/// Nieużywane.
		/// </summary>
		Interfaces.IInput IContainer.Input
		{
			get { return this.Data.Input; }
			set { }
		}

		/// <summary>
		/// Nieużywane.
		/// </summary>
		Interfaces.Graphics.IRenderer IContainer.Renderer
		{
			get { return this.Data.Renderer; }
			set { }
		}

		/// <summary>
		/// Kontrolki w tej kontrolce.
		/// </summary>
		public Interfaces.Graphics.Gui.IControlsCollection Controls { get; private set; }

		/// <summary>
		/// Sprawdza kontrolkę w 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int Control(string id)
		{
			return this.Controls[id].Check();
		}
		#endregion

		#region ControlBase Members
		protected override IContainer Owner
		{
			get { return base.Owner; }
			set
			{
				base.Owner = value;
				this.Controls.UpdateOwner();
			}
		}
		#endregion

		#region Unused
		public override bool PermanentActive { get { return false; } }

		public override int Check()
		{
			return 0;
		}

		public override bool ContainsMouse()
		{
			return false;
		}
		#endregion

		#region Constructors
		public Panel()
		{
			this.Controls = new Gui.Internals.ControlsCollection(this);
		}
		#endregion
	}
}
