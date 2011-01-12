using System.Windows.Markup;
using System.ComponentModel;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Objects;
	using Interfaces.Graphics.Gui.Layout;

	/// <summary>
	/// Klasa bazowa dla kontrolek, które można stylizować za pomocą obiektów(<see cref="IObject"/>).
	/// </summary>
	[ContentProperty("Objects")]
	public abstract class StylizableControlBase
		: ControlBase, IStylizableControl
	{
		#region Private fields
		private ILayoutEngine _LayoutEngine = new Layout.DefaultLayout();
		#endregion

		#region IStylizableControl Members
		/// <summary>
		/// Kolekcja z obiektami renderera dla kontrolki.
		/// </summary>
		public IObjectsCollection Objects { get; private set; }

		/// <summary>
		/// Silnik używany do układania obiektów.
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
				this.Size = this.LayoutEngine.Layout(this.Objects, this.Size);
			}
		}
		#endregion

		#region IControl Members
		/// <summary>
		/// Rysuje widoczne obiekty.
		/// </summary>
		public override void Render()
		{
			foreach (var obj in this.Objects)
			{
				if (obj.Visible)
				{
					obj.Render();
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
			this.Size = this.LayoutEngine.Layout(this.Objects, this.Size);
			base.EndInit();
		}
		#endregion

		#region Constructors
		public StylizableControlBase()
		{
			this.Objects = new Internals.ObjectsCollection(this);
		}
		#endregion
	}
}
