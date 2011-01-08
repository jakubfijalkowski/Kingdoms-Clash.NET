namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	using Layout;

	/// <summary>
	/// Kontrolka, która obsługuje układanie elementów.
	/// </summary>
	public interface ILayoutControl
		: IControl
	{
		/// <summary>
		/// Silnik używany do układania.
		/// </summary>
		ILayoutEngine LayoutEngine { get; set; }

		/// <summary>
		/// Wymusza ponowne rozłożenie elementów.
		/// </summary>
		void Layout();
	}
}
