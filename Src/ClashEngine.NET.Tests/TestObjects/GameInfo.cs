using ClashEngine.NET.Interfaces;

namespace ClashEngine.NET.Tests.TestObjects
{
	public class GameInfo
		: IGameInfo
	{
		#region IGameInfo Members
		public IWindow MainWindow { get; set; }
		public IScreensManager Screens { get; set; }
		public IResourcesManager Content { get; set; }
		public Interfaces.Graphics.IRenderer Renderer { get; set; }
		#endregion
	}
}
