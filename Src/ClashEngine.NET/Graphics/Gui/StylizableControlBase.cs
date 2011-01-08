using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Objects;

	/// <summary>
	/// Klasa bazowa dla kontrolek, które można stylizować za pomocą obiektów(<see cref="IObject"/>).
	/// </summary>
	[ContentProperty("Objects")]
	public abstract class StylizableControlBase
		: ControlBase, IStylizableControl
	{
		#region IStylizableControl Members
		/// <summary>
		/// Kolekcja z obiektami renderera dla kontrolki.
		/// </summary>
		public IObjectsCollection Objects { get; private set;}
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

		#region Constructors
		public StylizableControlBase()
		{
			this.Objects = new Internals.ObjectsCollection(this);
		}
		#endregion
	}
}
