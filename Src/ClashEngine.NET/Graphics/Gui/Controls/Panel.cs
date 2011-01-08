using System.ComponentModel;
using System.Windows.Markup;
using OpenTK.Input;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Controls;
	using Interfaces.Graphics.Gui.Layout;

	/// <summary>
	/// Panel - nie uczestniczy w interakcji z użytkownikiem, jest "statyczny".
	/// </summary>
	[ContentProperty("Controls")]
	public class Panel
		: ControlBase, IPanel
	{
		#region Private fields
		private ILayoutEngine _LayoutEngine = new Layout.DefaultLayout();
		#endregion

		#region IContainerControl Members
		/// <summary>
		/// Kolekcja kontrolek.
		/// </summary>
		public IControlsCollection Controls { get; private set; }

		/// <summary>
		/// Silnik używany do układania.
		/// </summary>
		[TypeConverter(typeof(Converters.LayoutConverter))]
		public ILayoutEngine LayoutEngine
		{
			get { return this._LayoutEngine; }
			set
			{
				if (value == null)
				{
					value = new Layout.DefaultLayout();
				}
				this._LayoutEngine = value;
				this.Layout();
			}
		}

		/// <summary>
		/// Wymusza ponowne rozłożenie elementów.
		/// </summary>
		public void Layout()
		{
			if (base.IsInitialized)
			{
				this.Size = this.LayoutEngine.Layout(this.Controls, this.Size);
			}
		}

		/// <summary>
		/// Sprawdza stan kontrolki za pomocą <see cref="IGuiControl.Check"/>.
		/// </summary>
		/// <param name="id">Identyfikator kontrolki.</param>
		/// <returns>Nr akcji bądź 0, gdy żadna akcja nie zaszła.</returns>
		public int Control(string id)
		{
			return this.Controls.Get(id).Check();
		}
		#endregion

		#region IControl Members
		/// <summary>
		/// Nie potrzebujemy być aktywni.
		/// </summary>
		public override bool PermanentActive { get { return false; } }

		/// <summary>
		/// Rysujemy kontrolki.
		/// </summary>
		public override void Render()
		{
			foreach (var control in this.Controls)
			{
				//Tylko widoczne i swoje.
				if (control.Visible && control.Owner == this)
				{
					control.Render();
				}
			}
		}

		/// <summary>
		/// Aktualizujemy kontrolki.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.Data.Hot = null;
			foreach (var control in this.Controls)
			{
				//Tylko widoczne i swoje.
				if (control.Visible && control.Owner == this)
				{
					if (control.ContainsMouse())
					{
						this.Data.Hot = control;
					}
					if (this.Data.Input[MouseButton.Left])
					{
						this.Data.Active = this.Data.Hot;
					}
					else if (this.Data.Active != null && !this.Data.Active.PermanentActive)
					{
						this.Data.Active = null;
					}
					control.Update(delta);
				}
			}
		}

		/// <summary>
		/// Nieużywane.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			return 0;
		}

		/// <summary>
		/// Nie potrzebujemy kiedykolwiek zmieniać stanu.
		/// </summary>
		/// <returns></returns>
		public override bool ContainsMouse()
		{
			return false;
		}

		/// <summary>
		/// Usuwamy własne kontrolki z właściciela.
		/// </summary>
		public override void OnRemove()
		{
			if (this.Owner != null)
			{
				foreach (var control in this.Controls)
				{
					this.Owner.Controls.Remove(control);
				}
			}
		}
		#endregion

		#region ISupportInitialize Members
		/// <summary>
		/// Układa kontrolki.
		/// </summary>
		public override void EndInit()
		{
			this.Size = this.LayoutEngine.Layout(this.Controls, this.Size);
			base.EndInit();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		public Panel()
		{
			this.Controls = new Gui.Internals.ControlsCollection(this);
		}
		#endregion
	}
}
