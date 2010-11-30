namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontrolka, która posiada obiekty renderera.
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
