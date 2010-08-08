namespace ClashEngine.NET
{
	/// <summary>
	/// Bazowa klasa dla komponentów które potrafią się renderować.
	/// </summary>
	public abstract class RenderableComponent
		: Component
	{
		/// <summary>
		/// Inicjalizuje nowy komponent.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public RenderableComponent(string id)
			: base(id)
		{ }

		/// <summary>
		/// Odrysowywuje kontrolkę.
		/// </summary>
		public abstract	void Render();
	}
}
