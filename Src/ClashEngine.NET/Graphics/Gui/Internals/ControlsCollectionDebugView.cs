using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui.Internals
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Proxy dla debuggera. Umożliwia przyjemne dla oka wyświetlanie listy kontrolek.
	/// </summary>
	internal sealed class ControlsCollectionDebugView
	{
		private readonly ControlsCollection Collection;

		public ControlsCollectionDebugView(ControlsCollection collection)
		{
			this.Collection = collection;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public IControl[] Items
		{
			get
			{
				IControl[] items = new IControl[this.Collection.Count];
				this.Collection.CopyTo(items, 0);
				return items;
			}
		}
	}
}
