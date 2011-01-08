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
		#region ILeftToRightLayout Members
		/// <summary>
		/// Czy elementy mają się układać w jedną linie(true) czy w wiele(false).
		/// </summary>
		public bool Singleline { get; set; }

		/// <summary>
		/// W którą stronę wyrównywać elementy - lewą(false) czy prawą(true).
		/// </summary>
		public bool AlignRight { get; set; }
		#endregion

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
				if (!this.Singleline && currPos.X + elements[i].Size.X > size.X) //Element nie mieści się w linii
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

			if(this.Singleline)
			{
				size.X = Math.Max(size.X, elements[elements.Count - 1].Position.X + elements[elements.Count - 1].Size.X);
			}
			size.Y = Math.Max(size.Y, currPos.Y + (currPos.X != 0 ? rowHeight : 0));

			if (this.AlignRight)
			{
				float usedX = 0;
				for (int i = elements.Count - 1; i >= 0; i--)
				{
					bool needReset = elements[i].Position.X == 0; //To był ostatni element linii

					elements[i].Position = new Vector2(size.X - usedX - elements[i].Size.X, elements[i].Position.Y);
					usedX += elements[i].Size.X;

					if (needReset)
					{
						usedX = 0;
					}
				}
			}

			return size;
		}
		#endregion
	}
}
