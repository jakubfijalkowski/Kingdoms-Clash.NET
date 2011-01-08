using ClashEngine.NET.Graphics.Gui;
using ClashEngine.NET.Graphics.Gui.Controls;
using ClashEngine.NET.Interfaces.Graphics.Gui;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	using TestObjects;

	[TestFixture(Description = "Testy dla panelu GUI")]
	public class GuiPanelTests
	{
		private Container Container;
		private Panel Panel;
		private Mock<Control> Control1;
		private Mock<Control> Control2;

		[SetUp]
		public void SetUp()
		{
			GameInfo gameInfo = new GameInfo();
			gameInfo.Mock();
			this.Container = new Container(gameInfo);

			this.Panel = new Panel();
			this.Panel.Id = "Panel";

			this.Control1 = new Mock<Control>("Control1");
			this.Control1.SetupAllProperties();

			this.Control2 = new Mock<Control>("Control2");
			this.Control2.SetupAllProperties();
			this.Control2.Object.Visible = true;

			this.Container.Root.Controls.Add(this.Panel);
			this.Panel.Controls.Add(this.Control1.Object);
			this.Panel.Controls.Add(this.Control2.Object);
		}

		[Test]
		public void PanelAddsControlsToOwner()
		{
			Assert.AreEqual(this.Container.Root.Controls.TotalCount, 3);
			Assert.AreEqual(this.Container.Root, (this.Panel as IControl).Owner);
			Assert.AreEqual(this.Panel, this.Control1.Object.Owner);
			Assert.AreEqual(this.Panel, this.Control2.Object.Owner);
		}

		[Test]
		public void PanelUpdatesItsControls1([Range(0.0, 1.0, 1.0)] double delta)
		{
			this.Control1.Setup(c => c.Update(delta));
			this.Control2.Setup(c => c.Update(delta));
			this.Panel.Update(delta);
			this.Control1.Verify(c => c.Update(delta), Times.Never());
			this.Control2.Verify(c => c.Update(delta), Times.Once());
		}

		[Test]
		public void PanelRendersItsControls1()
		{
			this.Control1.Setup(c => c.Render());
			this.Control2.Setup(c => c.Render());
			this.Panel.Render();
			this.Control1.Verify(c => c.Render(), Times.Never());
			this.Control2.Verify(c => c.Render(), Times.Once());
		}

		[Test]
		public void PanelUpdatesItsControls2([Range(0.0, 1.0, 1.0)] double delta)
		{
			this.Control1.Setup(c => c.Update(delta));
			this.Control2.Setup(c => c.Update(delta));
			this.Container.Update(delta);
			this.Control1.Verify(c => c.Update(delta), Times.Never());
			this.Control2.Verify(c => c.Update(delta), Times.Once());
		}

		[Test]
		public void PanelRendersItsControls2()
		{
			this.Control1.Setup(c => c.Render());
			this.Control2.Setup(c => c.Render());
			this.Container.Render();
			this.Control1.Verify(c => c.Render(), Times.Never());
			this.Control2.Verify(c => c.Render(), Times.Once());
		}

		[Test]
		public void PanelRemovesHisControlsFromOwner()
		{
			int oldCountPanel = this.Panel.Controls.Count;
			int oldCountContainer = this.Container.Root.Controls.TotalCount;
			Assert.IsTrue(this.Panel.Controls.Remove(this.Control1.Object), "This should not fail");
			Assert.AreEqual(oldCountPanel - 1, this.Panel.Controls.Count);
			Assert.AreEqual(oldCountContainer - 1, this.Container.Root.Controls.TotalCount);
		}

		[Test]
		public void RemovingPanelRemovesControlFromOwner()
		{
			Assert.IsTrue(this.Container.Root.Controls.Remove(this.Panel), "This should not fail");
			Assert.AreEqual(0, this.Container.Root.Controls.Count);
		}
	}
}
