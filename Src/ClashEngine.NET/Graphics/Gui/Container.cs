namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces;
	using Interfaces.Graphics;
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener GUI.
	/// </summary>
	public class Container
		: IContainer
	{
		#region IContainer Members
		/// <summary>
		/// Informacje o grze.
		/// </summary>
		public IGameInfo GameInfo { get; private set; }

		/// <summary>
		/// Główna kontrolka.
		/// </summary>
		/// <remarks>
		/// W tej implementacji jest ona typu <see cref="Controls.Panel"/>.
		/// </remarks>
		public IContainerControl Root { get; private set; }

		/// <summary>
		/// Uaktualnia wszystkie kontrolki w kontenerze.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			this.Root.Update(delta);
		}

		/// <summary>
		/// Renderuje wszystkie kontrolki.
		/// </summary>
		public void Render()
		{
			var oldSortMode = this.GameInfo.Renderer.SortMode;
			this.GameInfo.Renderer.SortMode = SortMode.FrontToBack;
			this.Root.Render();
			this.GameInfo.Renderer.SortMode = oldSortMode;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontener.
		/// </summary>
		/// <param name="gameInfo">Informacje o grze.</param>
		public Container(IGameInfo gameInfo)
		{
			this.GameInfo = gameInfo;
			this.Root = new Controls.Panel() { Id = "Root", Size = gameInfo.MainWindow.Size };
			this.Root.Data = new Internals.UIData(gameInfo.MainWindow.Input, gameInfo.Renderer);
		}
		#endregion
	}
}
