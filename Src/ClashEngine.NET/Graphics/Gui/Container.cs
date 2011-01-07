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
		/// Kamera używana przez kontener.
		/// Może być null.
		/// </summary>
		public ICamera Camera { get; set; }

		/// <summary>
		/// Główna kontrolka.
		/// </summary>
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
			this.GameInfo.Renderer.Camera = this.Camera;
			this.Root.Render();
		}
		#endregion

		#region Constructors
		public Container(IGameInfo gameInfo)
		{
			this.GameInfo = gameInfo;
			this.Root = new Controls.Panel();
			this.Root.Data = new Internals.UIData(gameInfo.MainWindow.Input, gameInfo.Renderer);
		}
		#endregion
	}
}
