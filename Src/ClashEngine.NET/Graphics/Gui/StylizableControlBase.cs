using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

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

		#region Constructors
		public StylizableControlBase()
		{
			this.Objects = new Internals.ObjectsCollection(this);
		}
		#endregion
	}
}
