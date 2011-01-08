using System.Collections.Generic;
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
		private Mock<Control> RootControl;
		private Mock<IUIData> Data;
		private Mock<Control> Control;
		private Mock<Control> ChildControl;

		private IControlsCollection Controls
		{
			get { return this.RootControl.Object.Controls; }
			set { this.RootControl.Object.Controls = value; }
		}

		[SetUp]
		public void SetUp()
		{
			this.RootControl = new Mock<Control>("Root");
			this.RootControl.SetupAllProperties();
			this.RootControl.Object.Data = (this.Data = new Mock<IUIData>()).Object;

			this.Control = new Mock<Control>("Control");
			this.Control.SetupAllProperties();
			this.Control.Setup(c => c.OnAdd());

			this.ChildControl = new Mock<Control>("ChildControl");
			this.ChildControl.SetupAllProperties();
			this.ChildControl.Setup(c => c.OnAdd());

			this.Controls = new ControlsCollection(this.RootControl.Object);
			this.Controls.Add(this.Control.Object);

			this.ChildControl.Object.Owner = this.Control.Object;
			this.Controls.AddChild(this.ChildControl.Object);
		}

		[Test]
		public void OwnerIsCorrect()
		{
			Assert.AreEqual(this.RootControl.Object, this.Control.Object.Owner);
			Assert.AreEqual(this.Control.Object, this.ChildControl.Object.Owner);
			this.Control.Verify(c => c.OnAdd(), Times.Once());
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
			ctrl1.Setup(c => c.OnAdd());
			ctrl2.Setup(c => c.OnAdd());

			int oldCount = this.Controls.Count;
			this.Controls.AddRange(new IControl[] { ctrl1.Object, ctrl2.Object });
			Assert.AreEqual(oldCount + 2, this.Controls.Count);
			Assert.AreEqual(this.RootControl.Object, ctrl1.Object.Owner);
			Assert.AreEqual(this.RootControl.Object, ctrl2.Object.Owner);
			ctrl1.Verify(c => c.OnAdd(), Times.Once());
			ctrl2.Verify(c => c.OnAdd(), Times.Once());
		}

		[Test]
		public void RemoveByItemWorks()
		{
			this.Control.Setup(c => c.OnRemove());
			int oldCount = this.Controls.Count;
			Assert.IsTrue(this.Controls.Remove(this.Control.Object));
			Assert.AreEqual(oldCount - 1, this.Controls.Count);
			this.Control.Verify(c => c.OnRemove(), Times.Once());
		}

		[Test]
		public void RemoveByIdWorks()
		{
			this.Control.Setup(c => c.OnRemove());
			int oldCount = this.Controls.Count;
			Assert.IsTrue(this.Controls.Remove("Control"));
			Assert.AreEqual(oldCount - 1, this.Controls.Count);
			this.Control.Verify(c => c.OnRemove(), Times.Once());
		}

		[Test]
		public void ContainsWorks()
		{
			Assert.IsTrue(this.Controls.Contains("Control"));
			Assert.IsFalse(this.Controls.Contains("ChildControl"));
		}

		[Test]
		public void IndexerWorks()
		{
			Assert.AreEqual(this.Control.Object, this.Controls["Control"]);
			Assert.Throws<KeyNotFoundException>(() => { var temp = this.Controls["ChildControl"]; });
		}
	}
}
