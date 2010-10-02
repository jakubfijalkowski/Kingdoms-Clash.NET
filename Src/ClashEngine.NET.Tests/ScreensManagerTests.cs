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
		private ScreensManager.ScreensManager Manager { get; set; }

		private Mock<Screen> Screen1 { get; set; }
		private Mock<Screen> Screen3 { get; set; }
		private Mock<Screen> Screen2 { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.Manager = new ScreensManager.ScreensManager(false);
			this.Screen1 = new Mock<Screen>();
			this.Screen1.Setup(s => s.OnInit());
			this.Screen1.Setup(s => s.OnDeinit());
			this.Screen1.Object.IsFullscreen = true;

			this.Screen2 = new Mock<Screen>();
			this.Screen2.Setup(s => s.OnInit());
			this.Screen2.Setup(s => s.OnDeinit());

			this.Screen3 = new Mock<Screen>();
			this.Screen3.Setup(s => s.OnInit());
			this.Screen3.Setup(s => s.OnDeinit());
			this.Screen3.Object.IsFullscreen = true;

			this.Manager.Add(this.Screen1.Object);
			this.Manager.Add(this.Screen2.Object);
			this.Manager.Add(this.Screen3.Object);
		}

		#region Adding/removing
		[Test]
		public void ScreensAdded()
		{
			Assert.AreEqual(3, this.Manager.Count);
		}

		[Test]
		public void ScreensInitialized()
		{
			this.Screen1.Verify(s => s.OnInit());
			this.Screen2.Verify(s => s.OnInit());
			this.Screen3.Verify(s => s.OnInit());
		}

		[Test]
		public void ScreensAddedInCorrectOrder()
		{
			Assert.AreEqual(this.Screen1.Object, this.Manager[0]);
			Assert.AreEqual(this.Screen2.Object, this.Manager[1]);
			Assert.AreEqual(this.Screen3.Object, this.Manager[2]);
		}

		[Test]
		public void ScreenRemoves()
		{
			int oldCount = this.Manager.Count;
			this.Manager.Remove(this.Screen3.Object);
			this.Screen3.Verify(s => s.OnDeinit());
			Assert.AreEqual(oldCount - 1, this.Manager.Count);

			//Wracamy do stanu sprzed - czy takie rozwiązanie jest poprawne? Jak to inaczej sprawdzić?
			this.Manager.Add(this.Screen3.Object);
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
			Assert.IsFalse(this.Manager.Remove(new Mock<Screen>().Object));
		}
		#endregion

		#region List management
		[Test]
		public void DoesScreenMoveToFront()
		{
			this.Manager.MoveToFront(this.Screen1.Object);
			Assert.AreEqual(this.Screen1.Object, this.Manager[0]);
		}

		[Test]
		public void DoesScreenMoveToSpecifiedPosition()
		{
			this.Manager.MoveTo(this.Screen1.Object, 2);
			Assert.AreEqual(this.Screen1.Object, this.Manager[1]);
		}

		[Test]
		public void ThrowsExectpionOnMovingToFrontNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.MoveToFront(null));
		}

		[Test]
		public void ThrowsExectpionOnMovingNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.MoveTo(null, 0));
		}

		[Test]
		public void ThrowsExectpionOnMovingToFrontNonExistingScreen()
		{
			Assert.Throws<Exceptions.ArgumentNotExistsException>(() => this.Manager.MoveToFront(new Mock<Screen>().Object));
		}

		[Test]
		public void ThrowsExectpionOnMovingNonExistingScreen()
		{
			Assert.Throws<Exceptions.ArgumentNotExistsException>(() => this.Manager.MoveTo(new Mock<Screen>().Object, 0));
		}
		#endregion

		#region State changing
		[Test]
		public void DoesScreenClose()
		{
			this.Screen1.Setup(s => s.StateChanged(ScreenState.Closed));

			this.Manager.MakeActive(this.Screen1.Object);
			this.Manager.Close(this.Screen1.Object);

			Assert.AreEqual(ScreenState.Closed, this.Screen1.Object.State);
			this.Screen1.Verify();
		}

		[Test]
		public void DoesScreenMakeActive()
		{
			this.Screen2.Setup(s => s.StateChanged(ScreenState.Closed));
			this.Manager.MakeActive(this.Screen2.Object);

			Assert.AreEqual(ScreenState.Active, this.Screen2.Object.State);
			this.Screen2.Verify(s => s.StateChanged(ScreenState.Closed));
		}

		[Test]
		public void DoesScreenMakeInactive()
		{
			this.Screen3.Setup(s => s.StateChanged(ScreenState.Closed));
			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeInactive(this.Screen3.Object);

			Assert.AreEqual(ScreenState.Inactive, this.Screen3.Object.State);
			this.Screen3.Verify(s => s.StateChanged(ScreenState.Closed));
		}

		[Test]
		public void ActivatingScreenCorrectlyDeactivateOther()
		{
			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeActive(this.Screen2.Object);
			this.Manager.MakeActive(this.Screen1.Object);
			Assert.AreEqual(ScreenState.Inactive, this.Screen2.Object.State);
			Assert.AreEqual(ScreenState.Inactive, this.Screen3.Object.State);
		}

		[Test]
		public void DeactivatingScreenCorrectlyActivateOther()
		{
			//Najpierw aktywujemy
			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeActive(this.Screen2.Object);
			this.Manager.MakeActive(this.Screen1.Object);

			this.Manager.MakeInactive(this.Screen1.Object);
			Assert.AreEqual(ScreenState.Active, this.Screen2.Object.State);
			Assert.AreEqual(ScreenState.Inactive, this.Screen3.Object.State);
		}
		#endregion

		#region Rendering and updating
		[Test]
		public void IsCorrectScreenUpdated1([Values(1.0)] double delta)
		{
			this.Screen1.Setup(s => s.Update(delta));

			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeActive(this.Screen2.Object);
			this.Manager.MakeActive(this.Screen1.Object);

			this.Manager.Update(delta);

			this.Screen1.Verify(s => s.Update(delta));
		}

		[Test]
		public void IsCorrectScreenUpdated2([Values(1.0)] double delta)
		{
			this.Screen2.Setup(s => s.Update(delta));

			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeActive(this.Screen2.Object);
			this.Manager.MakeInactive(this.Screen1.Object);

			this.Manager.Update(delta);

			this.Screen2.Verify(s => s.Update(delta));
		}

		[Test]
		public void IsCorrectScreenRendered()
		{
			this.Screen1.Setup(s => s.Render());

			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeActive(this.Screen2.Object);
			this.Manager.MakeActive(this.Screen1.Object);

			this.Manager.Render();

			this.Screen1.Verify(s => s.Render());
		}

		[Test]
		public void AreCorrectScreensRendered()
		{
			this.Screen2.Setup(s => s.Render());
			this.Screen3.Setup(s => s.Render());

			this.Manager.MakeActive(this.Screen3.Object);
			this.Manager.MakeActive(this.Screen2.Object);
			this.Manager.MakeInactive(this.Screen1.Object);

			this.Manager.Render();

			this.Screen3.Verify(s => s.Render());
			this.Screen2.Verify(s => s.Render());
		}
		#endregion
	}
}
