using ClashEngine.NET.EntitiesManager;
using NUnit.Framework;
using Moq;
using System;

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
			this.Entity2 = new Mock<GameEntity>("Entity2");
			this.Entity3 = new Mock<GameEntity>("Entity3");

			this.Manager.AddEntity(this.Entity1.Object);
			this.Manager.AddEntity(this.Entity2.Object);
			this.Manager.AddEntity(this.Entity3.Object);
		}

		#region Adding/removing
		[Test]
		public void DoEntitesAdd()
		{
			Assert.AreEqual(3, this.Manager.Entities.Count);
		}

		[Test]
		public void DoEntitiesRemove()
		{
			int cnt = this.Manager.Entities.Count;
			this.Manager.RemoveEntity(this.Entity1.Object);
			Assert.AreEqual(cnt - 1, this.Manager.Entities.Count);

			//Powrót do poprzedniego stanu
			this.Manager.AddEntity(this.Entity1.Object);
		}

		[Test]
		public void AddingNullThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.AddEntity(null));
		}

		[Test]
		public void RemovingNullThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.RemoveEntity(null));
		}

		[Test]
		public void AddingExistingEntityThrowsException()
		{
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Manager.AddEntity(this.Entity3.Object));
		}

		[Test]
		public void RemovingNonExistingEntityThrowsException()
		{
			Assert.Throws<Exceptions.ArgumentNotExistsException>(() => this.Manager.RemoveEntity(new Mock<GameEntity>("TmpEntity").Object));
		}
		#endregion

		#region Updating/Rendering
		[Test]
		public void DoAllEntitesUpdate([Values(1.0)] double delta)
		{
			this.Entity1.Setup(e => e.Update(delta));
			this.Entity2.Setup(e => e.Update(delta));
			this.Entity3.Setup(e => e.Update(delta));

			this.Manager.Update(delta);

			this.Entity1.VerifyAll();
			this.Entity2.VerifyAll();
			this.Entity3.VerifyAll();
		}
		#endregion
	}
}
