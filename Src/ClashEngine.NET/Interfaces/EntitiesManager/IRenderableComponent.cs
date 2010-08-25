namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla komponentów które potrafią się odrysować.
	/// </summary>
	public interface IRenderableComponent
		: IComponent
	{
		/// <summary>
		/// Odrysowywuje kontrolkę.
		/// </summary>
		void Render();
	}
}
