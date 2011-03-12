namespace ClashEngine.NET.Interfaces.Graphics.Components
{
	/// <summary>
	/// Komponent do bezpośredniego dodawania komponentów do encji.
	/// </summary>
	public interface IObjectHolder
		: EntitiesManager.IRenderableComponent
	{
		/// <summary>
		/// Obiekt.
		/// </summary>
		IObject Object { get; }
	}
}
