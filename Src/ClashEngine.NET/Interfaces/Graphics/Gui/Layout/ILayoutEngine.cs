﻿using System.Collections;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Layout
{
	/// <summary>
	/// Silnik odpowiedzialny za układanie kontrolek/obiektów.
	/// </summary>
	public interface ILayoutEngine
	{
		/// <summary>
		/// Układa wszystkie elementy.
		/// </summary>
		/// <remarks>
		/// Możemy mieć pewność, że poszczególne elementu listy dziedziczą z IPositionableElement.
		/// </remarks>
		/// <param name="elements">Lista elementów do ułożenia.</param>
		void Layout(IList elements);
	}
}