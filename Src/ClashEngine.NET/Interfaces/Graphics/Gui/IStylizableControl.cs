namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	using Objects;
	using Layout;

	/// <summary>
	/// Kontrolka, którą można stylizować za pomocą obiektów(<see cref="IObject"/>).
	/// </summary>
	public interface IStylizableControl
		: IControl
	{
		/// <summary>
		/// Kolekcja z obiektami renderera dla kontrolki.
		/// </summary>
		IObjectsCollection Objects { get; }

		/// <summary>
		/// Silnik używany do układania obiektów.
		/// </summary>
		ILayoutEngine LayoutEngine { get; set; }

		/// <summary>
		/// Wymusza ponowne rozłożenie elementów.
		/// </summary>
		void Layout();
	}
}
