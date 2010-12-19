using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Panel - nie uczestniczy w interakcji z użytkownikiem, jest "statyczny".
	/// </summary>
	[ContentProperty("Controls")]
	public class Panel
		: ControlBase, IPanel
	{
		#region IContainer Members
		/// <summary>
		/// Nieużywane - kontrolki dziedziczą wejście po głównym kontenerze.
		/// </summary>
		Interfaces.IInput IContainer.Input
		{
			get { return this.Data.Input; }
			set { }
		}

		/// <summary>
		/// Nieużywane - kontrolki dziedziczą renderer po głównym kontenerze.
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
		/// Sprawdza kontrolkę. 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int Control(string id)
		{
			return this.Controls[id].Check();
		}
		#endregion

		#region ControlBase Members
		/// <summary>
		/// Przy zmianie uaktualnia rodzica swoim kontrolkom.
		/// </summary>
		protected override IContainer Owner
		{
			get { return base.Owner; }
			set
			{
				base.Owner = value;
				(this.Controls as Gui.Internals.ControlsCollection).UpdateOwner();
			}
		}

		/// <summary>
		/// Przy zmianie zmienia offset swoim kontrolką.
		/// </summary>
		public override OpenTK.Vector2 AbsolutePosition
		{
			get	{ return base.AbsolutePosition; }
			protected set
			{
				base.AbsolutePosition = value;
				(this.Controls as Gui.Internals.ControlsCollection).SetOffset(base.AbsolutePosition);
			}
		}

		/// <summary>
		/// Usuwa własne kontrolki z rodzica.
		/// </summary>
		public override void OnRemove()
		{
			foreach (var c in this.Controls)
			{
				this.Owner.Controls.Remove(c);
			}
		}
		#endregion

		#region Unused
		/// <summary>
		/// Nie potrzebujemy zdarzeń - zawsze false.
		/// </summary>
		public override bool PermanentActive { get { return false; } }

		/// <summary>
		/// Nie potrzebujemy zdarzeń - zawsze zero.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			return 0;
		}

		/// <summary>
		/// Nie potrzebujemy zdarzeń - zawsze false.
		/// </summary>
		/// <returns></returns>
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
