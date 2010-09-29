using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla ComponentsCollection")]
	public class ComponentsCollectionTests
	{
		private IComponentsCollection Collection;

		private Mock<Component> Component1;
		private Mock<Component> Component2;
		private Mock<OtherComponent> Component3;

		[SetUp]
		public void SetUp()
		{
			GameEntity ent = new GameEntity("Test");
			this.Collection = new ComponentsCollection(ent);

			this.Component1 = new Mock<Component>("Component1") { CallBase = true };
			this.Component1.Setup(c => c.Init(ent));

			this.Component2 = new Mock<Component>("Component2") { CallBase = true };
			this.Component2.Setup(c => c.Init(ent));

			this.Component3 = new Mock<OtherComponent>("Component3") { CallBase = true };
			this.Component3.Setup(c => c.Init(ent));

			this.Collection.Add(this.Component1.Object);
			this.Collection.Add(this.Component2.Object);
			this.Collection.Add(this.Component3.Object);
		}

		[Test]
		public void AllComponentsAdded()
		{
			Assert.AreEqual(3, this.Collection.Count);
		}

		[Test]
		public void AddingSameComponentThrowsException()
		{
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Collection.Add(this.Component1.Object));
		}

		[Test]
		public void ComponentsAreInitialized()
		{
			this.Component1.VerifyAll();
			this.Component2.VerifyAll();
			this.Component3.VerifyAll();
		}

		[Test]
		public void GettingSingleComponentById()
		{
			Assert.AreEqual(this.Component1.Object, this.Collection["Component1"]);
			Assert.AreEqual(this.Component2.Object, this.Collection["Component2"]);
			Assert.AreEqual(this.Component3.Object, this.Collection["Component3"]);
		}

		[Test]
		public void GettingManyComponentsByTypeGeneric()
		{
			CollectionAssert.AreEquivalent(new IComponent[] { this.Component1.Object, this.Component2.Object }, this.Collection.Get<Component>());
			CollectionAssert.AreEquivalent(new IComponent[] { this.Component3.Object }, this.Collection.Get<OtherComponent>());
		}

		[Test]
		public void GettingManyComponentsByTypeNonGeneric()
		{
			CollectionAssert.AreEquivalent(new IComponent[] { this.Component1.Object, this.Component2.Object }, this.Collection.Get(typeof(Component)));
			CollectionAssert.AreEquivalent(new IComponent[] { this.Component3.Object }, this.Collection.Get(typeof(OtherComponent)));
		}

		[Test]
		public void GettingSingleComponentByTypeGeneric()
		{
			Assert.AreEqual(this.Component1.Object, this.Collection.GetSingle<Component>());
			Assert.AreEqual(this.Component3.Object, this.Collection.GetSingle<OtherComponent>());
		}

		[Test]
		public void GettingSingleComponentByTypeNonGeneric()
		{
			Assert.AreEqual(this.Component1.Object, this.Collection.GetSingle(typeof(Component)));
			Assert.AreEqual(this.Component3.Object, this.Collection.GetSingle(typeof(OtherComponent)));
		}
	}

	public abstract class OtherComponent
		: IComponent
	{
		#region IComponent Members
		public string Id { get; private set; }

		public IGameEntity Owner { get; private set; }

		public virtual void Init(IGameEntity owner)
		{
			this.Owner = owner;
		}

		public abstract void Update(double delta);
		#endregion

		public OtherComponent(string id)
		{
			this.Id = id;
		}
	}
}
