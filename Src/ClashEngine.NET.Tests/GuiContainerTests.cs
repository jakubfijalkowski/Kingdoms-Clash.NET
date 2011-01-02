using ClashEngine.NET.Graphics.Gui;
using ClashEngine.NET.Interfaces;
using ClashEngine.NET.Interfaces.Graphics;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	using TestObjects;

	[TestFixture(Description = "Testy dla kontenera Gui")]
	public class GuiContainerTests
	{
		private Container Container;
		private Mock<Control> Control1;
		private Mock<Control> Control2;

		[SetUp]
		public void SetUp()
		{
			GameInfo gameInfo = new GameInfo();
			gameInfo.Mock();
			this.Container = new Container(gameInfo);

			this.Control1 = new Mock<Control>("Control1");
			this.Control1.SetupAllProperties();
			this.Control1.Object.Visible = true;

			this.Control2 = new Mock<Control>("Control2");
			this.Control2.SetupAllProperties();

			this.Container.Controls.Add(this.Control1.Object);
			this.Container.Controls.Add(this.Control2.Object);
		}

		[Test]
		public void UpdatesCorrectControls([Range(0.0, 1.0, 1.0)] double delta)
		{
			this.Control1.Setup(c => c.Update(delta));
			this.Control2.Setup(c => c.Update(delta));
			this.Container.Update(delta);
			this.Control1.Verify(c => c.Update(delta), Times.Once());
			this.Control2.Verify(c => c.Update(delta), Times.Never());
		}

		[Test]
		public void RendersCorrectControls()
		{
			this.Control1.Setup(c => c.Render());
			this.Control2.Setup(c => c.Render());
			this.Container.Render();
			this.Control1.Verify(c => c.Render(), Times.Once());
			this.Control2.Verify(c => c.Render(), Times.Never());
		}
	}
}
