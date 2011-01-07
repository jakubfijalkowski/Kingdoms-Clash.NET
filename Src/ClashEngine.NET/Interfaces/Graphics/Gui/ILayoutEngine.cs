using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Silnik odpowiedzialny za układanie kontrolek/obiektów.
	/// </summary>
	public interface ILayoutEngine
	{
		/// <summary>
		/// Lista elementów.
		/// </summary>
		IList<IPositionableElement> Elements { get; set; }

		/// <summary>
		/// Uaktualnia wszystkie elementy.
		/// </summary>
		void Layout();
	}
}
