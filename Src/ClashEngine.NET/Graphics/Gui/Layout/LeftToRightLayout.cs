using System;
using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Layout
{
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Layout;

	/// <summary>
	/// Silnik układający elementy od lewej do prawej strony w wierszach o wysokości równej największej kontrolce w wierszu.
	/// </summary>
	public class LeftToRightLayout
		: ILeftToRightLayout
	{
		#region ILayoutEngine Members
		public Vector2 Layout<T>(IList<T> elements, Vector2 size)
			where T : IPositionableElement
		{
			float rowHeight = 0;
			Vector2 currPos = Vector2.Zero;

			for (int i = 0; i < elements.Count; i++)
			{
				if (!elements[i].Visible)
				{
					continue;
				}
				if (currPos.X + elements[i].Size.X > size.X) //Element nie mieści się
				{
					if (currPos.X == 0) //Element nie mieści się w pustym wierszu
					{
						size.X = elements[i].Size.X; //Już się mieści ;)
						elements[i].Position = currPos;
						rowHeight = Math.Max(rowHeight, elements[i].Size.Y);
					}
					else
					{
						--i; //Po przełamaniu linii będziemy mogli go jeszcze raz spróbować umieścić.
					}
					//Łamiemy linie
					currPos.X = 0;
					currPos.Y += rowHeight;
					continue;
				}
				rowHeight = Math.Max(rowHeight, elements[i].Size.Y);
				elements[i].Position = currPos;
				currPos.X += elements[i].Size.X;
			}

			size.Y = Math.Max(size.Y, currPos.Y + (currPos.X != 0 ? rowHeight : 0));

			return size;
		}
		#endregion
	}
}
