using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	[ContentProperty("Objects")]
	public abstract class ObjectControlBase
		: ControlBase, IObjectControl
	{
		#region IObjectControl Members
		/// <summary>
		/// Lista z obiektami dla renderera.
		/// </summary>
		public IObjectsCollection Objects { get; private set; }
		#endregion

		#region ControBase overrides
		/// <summary>
		/// Renderuje wszystkie obiekty.
		/// </summary>
		public override void Render()
		{
			foreach (var obj in this.Objects)
			{
				if (obj.Visible)
				{
					this.Data.Renderer.Draw(obj);
				}
			}
		}
		#endregion

		#region Constructors
		public ObjectControlBase()
		{
			this.Objects = new ObjectsCollection(this);
		}
		#endregion
	}
}
