using System;
using ClashEngine.NET.EntitiesManager;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy managera encji")]
	public class EntitiesManagerTests
	{
		private EntitiesManager.EntitiesManager Manager { get; set; }
		public Mock<GameEntity> Entity1 { get; set; }
		public Mock<GameEntity> Entity2 { get; set; }
		public Mock<GameEntity> Entity3 { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.Manager = new EntitiesManager.EntitiesManager();
			this.Entity1 = new Mock<GameEntity>("Entity1");
			this.Entity1.Setup(e => e.OnInit());
			this.Entity1.Setup(e => e.OnDeinit());

			this.Entity2 = new Mock<GameEntity>("Entity2");
			this.Entity2.Setup(e => e.OnInit());
			this.Entity2.Setup(e => e.OnDeinit());

			this.Entity3 = new Mock<GameEntity>("Entity3");
			this.Entity3.Setup(e => e.OnInit());
			this.Entity3.Setup(e => e.OnDeinit());

			this.Manager.Add(this.Entity1.Object);
			this.Manager.Add(this.Entity2.Object);
			this.Manager.Add(this.Entity3.Object);
		}

		#region Adding/removing
		[Test]
		public void EntitiesAdded()
		{
			Assert.AreEqual(3, this.Manager.Count);
		}

		[Test]
		public void EntitiesInitialized()
		{
			this.Entity1.Verify(e => e.OnInit());
			this.Entity2.Verify(e => e.OnInit());
			this.Entity3.Verify(e => e.OnInit());
		}

		[Test]
		public void EntitiesRemove()
		{
			int cnt = this.Manager.Count;
			this.Manager.Remove(this.Entity1.Object);
			Assert.AreEqual(cnt - 1, this.Manager.Count);
			this.Entity1.Verify(e => e.OnDeinit());

			//Powrót do poprzedniego stanu
			this.Manager.Add(this.Entity1.Object);
		}

		[Test]
		public void PropertiesAreCorrect()
		{
			Assert.AreEqual(this.Manager, this.Entity1.Object.Manager);
			Assert.AreEqual(this.Manager, this.Entity2.Object.Manager);
			Assert.AreEqual(this.Manager, this.Entity3.Object.Manager);
		}

		[Test]
		public void AddingNullThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Add(null));
		}

		[Test]
		public void RemovingNullThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Remove(null));
		}

		[Test]
		public void AddingExistingEntityThrowsException()
		{
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Manager.Add(this.Entity3.Object));
		}

		[Test]
		public void RemovingNonExistingEntityReturnsFalse()
		{
			Assert.IsFalse(this.Manager.Remove(new Mock<GameEntity>("TmpEntity").Object));
		}
		#endregion

		#region Updating/Rendering
		[Test]
		public void EntitiesUpdate([Values(1.0)] double delta)
		{
			this.Entity1.Setup(e => e.Update(delta));
			this.Entity2.Setup(e => e.Update(delta));
			this.Entity3.Setup(e => e.Update(delta));

			this.Manager.Update(delta);

			this.Entity1.Verify(e => e.Update(delta));
			this.Entity2.Verify(e => e.Update(delta));
			this.Entity3.Verify(e => e.Update(delta));
		}

		[Test]
		public void EntitiesRender()
		{
			this.Entity1.Setup(e => e.Render());
			this.Entity2.Setup(e => e.Render());
			this.Entity3.Setup(e => e.Render());

			this.Manager.Render();

			this.Entity1.Verify(e => e.Render());
			this.Entity2.Verify(e => e.Render());
			this.Entity3.Verify(e => e.Render());
		}
		#endregion
	}
}
