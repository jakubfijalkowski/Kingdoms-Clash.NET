namespace ClashEngine.NET.Interfaces.Graphics.Gui.Layout
{
	/// <summary>
	/// Silnik układający elementy od lewej do prawej strony w wierszach o wysokości równej największej kontrolce w wierszu.
	/// </summary>
	public interface ILeftToRightLayout
		: ILayoutEngine
	{
		/// <summary>
		/// Czy elementy mają się układać w jedną linie(true) czy w wiele(false).
		/// </summary>
		bool Singleline { get; set; }

		/// <summary>
		/// W którą stronę wyrównywać elementy - lewą(false) czy prawą(true).
		/// </summary>
		bool AlignRight { get; set; }
	}
}
