using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

using ClashEngine.NET.ScreensManager;
using ClashEngine.NET.Interfaces.ScreensManager;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy managera ekranów")]
	public class ScreensManagerTests
	{
		private ScreensManager.ScreensManager Manager;

		private Mock<Screen> Screen1; //Fullscreen
		private Mock<Screen> Screen2; //Popup
		private Mock<Screen> Screen3; //Normal
		private Mock<Screen> Screen4; //Normal

		private IScreen[] ScreensList;

		[SetUp]
		public void SetUp()
		{
			this.Manager = new ScreensManager.ScreensManager(null);
			this.Screen1 = new Mock<Screen>("Screen1", ScreenType.Fullscreen);
			this.Screen1.Setup(s => s.OnInit());
			this.Screen1.Setup(s => s.OnDeinit());

			this.Screen2 = new Mock<Screen>("Screen2", ScreenType.Popup);
			this.Screen2.Setup(s => s.OnInit());
			this.Screen2.Setup(s => s.OnDeinit());

			this.Screen3 = new Mock<Screen>("Screen3", ScreenType.Normal);
			this.Screen3.Setup(s => s.OnInit());
			this.Screen3.Setup(s => s.OnDeinit());

			this.Screen4 = new Mock<Screen>("Screen4", ScreenType.Normal);
			this.Screen4.Setup(s => s.OnInit());
			this.Screen4.Setup(s => s.OnDeinit());

			this.Manager.Add(this.Screen1.Object);
			this.Manager.Add(this.Screen2.Object);
			this.Manager.Add(this.Screen3.Object);
			this.Manager.Add(this.Screen4.Object);

			this.ScreensList = new IScreen[]
			{
				this.Screen1.Object,
				this.Screen2.Object,
				this.Screen3.Object,
				this.Screen4.Object
			};
		}

		[TearDown]
		public void TearDown()
		{
			this.Manager.Clear();
		}

		#region Adding/removing
		[Test]
		public void ScreensAdded()
		{
			Assert.AreEqual(4, this.Manager.Count);
		}

		[Test]
		public void ScreensInitialized()
		{
			this.Screen1.Verify(s => s.OnInit());
			this.Screen2.Verify(s => s.OnInit());
			this.Screen3.Verify(s => s.OnInit());
		}

		[Test]
		public void ScreenRemoves()
		{
			int oldCount = this.Manager.Count;
			this.Manager.Remove(this.Screen3.Object);
			this.Screen3.Verify(s => s.OnDeinit());
			Assert.AreEqual(oldCount - 1, this.Manager.Count);
		}

		[Test]
		public void ThrowsExectpionOnAddingNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Add(null));
		}

		[Test]
		public void ThrowsExectpionOnRemovingNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Remove(null));
		}

		[Test]
		public void ThrowsExectpionOnAddingExistingScreen()
		{
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Manager.Add(this.Screen1.Object));
		}

		[Test]
		public void RemovingNonExistingScreenReturnsFalse()
		{
			Assert.IsFalse(this.Manager.Remove(new Mock<Screen>("NonExisting", ScreenType.Normal).Object));
		}
		#endregion

		#region List management
		[Test]
		public void MovingNormalScreenToFrontCoversOthers()
		{
			this.Screen4.Object.Activate();
			this.Screen3.Object.Activate();
			this.Screen2.Object.Activate();
			this.Screen1.Object.Activate();

			this.Screen3.Object.MoveToFront();

			this.TestStates(new ScreenState[]
			{
				ScreenState.Covered,
				ScreenState.Hidden,
				ScreenState.Activated,
				ScreenState.Hidden
			});
		}

		[Test]
		public void MovingNormalToSecondPosCoversOthers()
		{
			this.Screen4.Object.Activate();
			this.Screen3.Object.Activate();
			this.Screen2.Object.Activate();

			this.Screen3.Object.MoveTo(1);

			this.TestStates(new ScreenState[]
			{
				ScreenState.Deactivated,
				ScreenState.Covered,
				ScreenState.Activated,
				ScreenState.Covered
			});
		}

		[Test]
		public void ThrowsExectpionOnMovingToFrontNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.MoveToFront(null as IScreen));
		}

		[Test]
		public void ThrowsExectpionOnMovingNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.MoveTo(null as IScreen, 0));
		}

		[Test]
		public void ThrowsExectpionOnMovingToFrontNonExistingScreen()
		{
			Assert.Throws<Exceptions.ArgumentNotExistsException>(() => this.Manager.MoveToFront(new Mock<Screen>("NonExisting", ScreenType.Normal).Object));
		}

		[Test]
		public void ThrowsExectpionOnMovingNonExistingScreen()
		{
			Assert.Throws<Exceptions.ArgumentNotExistsException>(() => this.Manager.MoveTo(new Mock<Screen>("NonExisting", ScreenType.Normal).Object, 0));
		}
		#endregion

		#region State changing
		[Test]
		public void ActivatingFullscreenHidesOthers()
		{
			this.Screen4.Object.Activate();
			this.Screen3.Object.Activate();
			this.Screen2.Object.Activate();
			this.Screen1.Object.Activate();

			this.TestStates(new ScreenState[]
			{
				ScreenState.Activated,
				ScreenState.Hidden,
				ScreenState.Hidden,
				ScreenState.Hidden,
			});
		}

		[Test]
		public void ActivatingPopupNotAffectOthers()
		{
			this.Screen4.Object.Activate();
			this.Screen3.Object.Activate();
			this.Screen2.Object.Activate();

			this.TestStates(new ScreenState[]
			{
				ScreenState.Deactivated,
				ScreenState.Activated,
				ScreenState.Activated,
				ScreenState.Covered
			});
		}

		[Test]
		public void ActivatingNormalScreenCoversOthers()
		{
			this.Screen4.Object.Activate();
			this.Screen3.Object.Activate();

			this.TestStates(new ScreenState[]
			{
				ScreenState.Deactivated,
				ScreenState.Deactivated,
				ScreenState.Activated,
				ScreenState.Covered
			});
		}
		#endregion

		#region Rendering and updating
		[Test]
		public void IsCorrectScreenUpdated1([Values(1.0)] double delta)
		{
			this.Screen1.Setup(s => s.Update(delta));

			this.Manager.Activate(this.Screen3.Object);
			this.Manager.Activate(this.Screen2.Object);
			this.Manager.Activate(this.Screen1.Object);

			this.Manager.Update(delta);

			this.Screen1.Verify(s => s.Update(delta));
		}

		[Test]
		public void IsCorrectScreenUpdated2([Values(1.0)] double delta)
		{
			this.Screen2.Setup(s => s.Update(delta));

			this.Manager.Activate(this.Screen3.Object);
			this.Manager.Activate(this.Screen2.Object);
			this.Manager.Deactivate(this.Screen1.Object);

			this.Manager.Update(delta);

			this.Screen2.Verify(s => s.Update(delta));
		}

		[Test]
		public void IsCorrectScreenRendered()
		{
			this.Screen1.Setup(s => s.Render());

			this.Manager.Activate(this.Screen3.Object);
			this.Manager.Activate(this.Screen2.Object);
			this.Manager.Activate(this.Screen1.Object);

			this.Manager.Render();

			this.Screen1.Verify(s => s.Render());
		}

		[Test]
		public void AreCorrectScreensRendered()
		{
			this.Screen2.Setup(s => s.Render());
			this.Screen3.Setup(s => s.Render());

			this.Manager.Activate(this.Screen3.Object);
			this.Manager.Activate(this.Screen2.Object);
			this.Manager.Deactivate(this.Screen1.Object);

			this.Manager.Render();

			this.Screen3.Verify(s => s.Render());
			this.Screen2.Verify(s => s.Render());
		}
		#endregion

		#region Utilities
		private void TestStates(ScreenState[] states)
		{
			if (states.Length != this.ScreensList.Length)
			{
				throw new ArgumentException("Invalid numberf elements", "states");
			}
			for (int i = 0; i < this.ScreensList.Length; i++)
			{
				Assert.AreEqual(states[i], this.ScreensList[i].State);
			}
		}
		#endregion
	}
}
