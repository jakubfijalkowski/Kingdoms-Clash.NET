using ClashEngine.NET.Graphics.Gui.Internals;
using ClashEngine.NET.Interfaces.Graphics.Gui;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	using TestObjects;

	[TestFixture(Description = "Testy dla klasy ControlsCollection")]
	public class ControlsCollectionTests
	{
		private ControlsCollection Controls;
		private Mock<IUIData> Data;
		private Mock<IContainer> Owner1;
		private Mock<IContainer> Owner2;
		private Mock<Control> Control;
		private Mock<Control> ChildControl;

		[SetUp]
		public void SetUp()
		{
			this.Data = new Mock<IUIData>();

			this.Owner1 = new Mock<IContainer>();
			this.Owner1.SetupAllProperties();

			this.Owner2 = new Mock<IContainer>();
			this.Owner2.SetupAllProperties();

			this.Control = new Mock<Control>("Control");
			this.Control.SetupAllProperties();

			this.ChildControl = new Mock<Control>("ChildControl");
			this.ChildControl.SetupAllProperties();

			this.Controls = new ControlsCollection(this.Owner1.Object, this.Data.Object);
			this.Controls.Add(this.Control.Object);

			this.ChildControl.Object.Owner = this.Owner2.Object;
			this.Controls.AddChildControl(this.ChildControl.Object);
		}

		[Test]
		public void OwnerIsCorrect()
		{
			Assert.AreEqual(this.Owner1.Object, this.Control.Object.Owner);
			Assert.AreEqual(this.Owner2.Object, this.ChildControl.Object.Owner);
		}

		[Test]
		public void DataIsCorrect()
		{
			Assert.AreEqual(this.Data.Object, this.Control.Object.Data);
			Assert.AreEqual(this.Data.Object, this.ChildControl.Object.Data);
		}

		[Test]
		public void AddRangeWorks()
		{
			Mock<Control> ctrl1 = new Mock<Control>("ctrl1"), ctrl2 = new Mock<Control>("ctrl2");
			ctrl1.SetupAllProperties();
			ctrl2.SetupAllProperties();

			int oldCount = this.Controls.Count;
			this.Controls.AddRange(new IControl[] { ctrl1.Object, ctrl2.Object });
			Assert.AreEqual(oldCount + 2, this.Controls.Count);
			Assert.AreEqual(this.Owner1.Object, ctrl1.Object.Owner);
			Assert.AreEqual(this.Owner1.Object, ctrl2.Object.Owner);
		}

		[Test]
		public void RemoveByItemWorks()
		{
			int oldCount = this.Controls.Count;
			Assert.IsTrue(this.Controls.Remove(this.Control.Object));
			Assert.AreEqual(oldCount - 1, this.Controls.Count);
		}

		[Test]
		public void RemoveByIdWorks()
		{
			int oldCount = this.Controls.Count;
			Assert.IsTrue(this.Controls.Remove("Control"));
			Assert.AreEqual(oldCount - 1, this.Controls.Count);
		}

		[Test]
		public void ContainsWorks()
		{
			Assert.IsTrue(this.Controls.Contains("Control"));
			Assert.IsTrue(this.Controls.Contains("ChildControl"));
		}

		[Test]
		public void IndexerWorks()
		{
			Assert.AreEqual(this.Control.Object, this.Controls["Control"]);
			Assert.AreEqual(this.ChildControl.Object, this.Controls["ChildControl"]);
		}

		[Test]
		public void SetOffsetWorks()
		{
			this.Controls.SetOffset(new OpenTK.Vector2(10, 10));
			Assert.AreEqual(new OpenTK.Vector2(10, 10), this.Control.Object.ContainerOffset);
			Assert.AreEqual(new OpenTK.Vector2(0, 0), this.ChildControl.Object.ContainerOffset);
		}
	}
}
