namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kontrolka, którą można stylizować za pomocą obiektów(<see cref="IObject"/>).
	/// </summary>
	public interface IStylizableControl
		: IControl
	{
		/// <summary>
		/// Kolekcja z obiektami renderera dla kontrolki.
		/// </summary>
		//IObjectsCollection Objects { get; }
	}
}
