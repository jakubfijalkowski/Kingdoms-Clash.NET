using ClashEngine.NET.Interfaces;
using Moq;

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

		public void Mock()
		{
			this.MainWindow = new Window();
			(this.MainWindow as Window).Input = new Mock<IInput>().Object;
			(this.MainWindow as Window).Size = new OpenTK.Vector2(1, 1);

			this.Screens = new Mock<IScreensManager>().Object;
			this.Content = new Mock<IResourcesManager>().Object;
			this.Renderer = new Mock<Interfaces.Graphics.IRenderer>().Object;
		}
	}
}
