namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Bazowa klasa dla komponentów które potrafią się renderować.
	/// </summary>
	public abstract class RenderableComponent
		: Component, IRenderableComponent
	{
		/// <summary>
		/// Renderer właściciela.
		/// </summary>
		protected Interfaces.Graphics.IRenderer Renderer { get { return this.Owner.GameInfo.Renderer; } }

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
