using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Layout
{
	using Interfaces.Graphics.Gui.Layout;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Domyślny layout - nie zmienia nic.
	/// </summary>
	public class DefaultLayout
		: IDefaultLayout
	{
		#region ILayoutEngine Members
		public Vector2 Layout<T>(IList<T> elements, Vector2 size)
			where T : IPositionableElement
		{
			return size;
		}
		#endregion
	}
}
