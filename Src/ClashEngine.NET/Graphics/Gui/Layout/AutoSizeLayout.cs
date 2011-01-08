using System;
using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Layout
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Layout;

	/// <summary>
	/// Dostosowuje rozmiar kontrolki do dzieci.
	/// </summary>
	public class AutoSizeLayout
		: IAutoSizeLayout
	{
		#region ILayoutEngine Members
		public Vector2 Layout<T>(IList<T> elements, Vector2 size)
			where T : IPositionableElement
		{
			var result = size;
			foreach (IPositionableElement item in elements)
			{
				result.X = Math.Max(result.X, item.Position.X + item.Size.X);
				result.Y = Math.Max(result.Y, item.Position.Y + item.Size.Y);
			}
			return result;
		}
		#endregion
	}
}
