namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontrolka GUI, której wygląd można definiować za pomocą "obiektów".
	/// </summary>
	public interface IObjectControl
		: IControl
	{
		/// <summary>
		/// Kolekcja z obiektami renderera dla kontrolki.
		/// </summary>
		IObjectsCollection Objects { get; }
	}
}
