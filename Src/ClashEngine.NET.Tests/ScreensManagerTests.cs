﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy managera ekranów")]
	public class ScreensManagerTests
	{
		private ScreensManager Manager { get; set; }

		private Mock<Screen> Screen1 { get; set; }
		private Mock<Screen> Screen3 { get; set; }
		private Mock<Screen> Screen2 { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.Manager = new ScreensManager();
			this.Screen1 = new Mock<Screen>();
			this.Screen3 = new Mock<Screen>();
			this.Screen2 = new Mock<Screen>();

			this.Manager.AddScreen(this.Screen1.Object);
			this.Manager.AddScreen(this.Screen2.Object);
			this.Manager.AddScreen(this.Screen3.Object);
		}

		#region Adding/removing
		[Test]
		public void DoScreensAdd()
		{
			Assert.AreEqual(3, this.Manager.Screens.Count);
		}

		[Test]
		public void DoScreensAddInCorrectOrder()
		{
			List<Screen> screensList = new List<Screen>();
			screensList.Add(Screen1.Object);
			screensList.Add(Screen2.Object);
			screensList.Add(Screen3.Object);

			CollectionAssert.AreEqual(screensList, this.Manager.Screens);
		}

		[Test]
		public void DoesScreenRemove()
		{
			int oldCount = this.Manager.Screens.Count;
			this.Manager.RemoveScreen(this.Screen3.Object);
			Assert.AreEqual(oldCount - 1, this.Manager.Screens.Count);

			//Wracamy do stanu sprzed - czy takie rozwiązanie jest poprawne? Jak to inaczej sprawdzić?
			this.Manager.AddScreen(this.Screen3.Object);
		}

		[Test]
		public void ThrowsExectpionOnAddingNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.AddScreen(null));
		}

		[Test]
		public void ThrowsExectpionOnRemovingNull()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.RemoveScreen(null));
		}

		[Test]
		public void ThrowsExectpionOnAddingExistingScreen()
		{
			Assert.Throws<Exceptions.AlreadyExistsException>(() => this.Manager.AddScreen(this.Screen1.Object));
		}

		[Test]
		public void ThrowsExectpionOnRemovingNonExistingScreen()
		{
			Assert.Throws<ArgumentException>(() => this.Manager.RemoveScreen(new Mock<Screen>().Object));
		}
		#endregion

		#region List management
		[Test]
		public void DoesScreenMoveToFront()
		{
			this.Manager.MoveToFront(this.Screen1.Object);
			Assert.AreEqual(this.Screen1.Object, this.Manager.Screens[0]);
		}

		[Test]
		public void DoesScreenMoveToSpecifiedPosition()
		{
			this.Manager.MoveTo(this.Screen1.Object, 2);
			Assert.AreEqual(this.Screen1.Object, this.Manager.Screens[1]);
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
			Assert.Throws<ArgumentException>(() => this.Manager.MoveToFront(new Mock<Screen>().Object));
		}

		[Test]
		public void ThrowsExectpionOnMovingNonExistingScreen()
		{
			Assert.Throws<ArgumentException>(() => this.Manager.MoveTo(new Mock<Screen>().Object, 0));
		}
		#endregion

		#region State changing
		[Test]
		public void DoesScreenClose()
		{
			this.Screen1.Setup(s => s.StateChanged());
			this.Manager.Close(this.Screen1.Object);

			Assert.AreEqual(ScreenState.Closed, this.Screen1.Object.State);
			this.Screen1.Verify();
		}

		[Test]
		public void DoesScreenMakeActive()
		{
			this.Screen2.Setup(s => s.StateChanged());
			this.Manager.MakeActive(this.Screen2.Object);

			Assert.AreEqual(ScreenState.Active, this.Screen2.Object.State);
			this.Screen2.Verify();
		}

		[Test]
		public void DoesScreenMakeInactive()
		{
			this.Screen3.Setup(s => s.StateChanged());
			this.Manager.MakeInactive(this.Screen3.Object);

			Assert.AreEqual(ScreenState.Inactive, this.Screen3.Object.State);
			this.Screen3.Verify();
		}
		#endregion
	}
}