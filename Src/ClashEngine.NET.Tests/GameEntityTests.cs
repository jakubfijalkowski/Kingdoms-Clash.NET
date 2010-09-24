using Moq;
using NUnit.Framework;

using ClashEngine.NET.EntitiesManager;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testowanie tworzenia encji i komponentów")]
	public class GameEntityTests
	{
		Mock<Component> Component;
		Mock<RenderableComponent> RenderableComponent;
		Mock<Attribute> Attribute;
		Mock<Attribute<int>> GenericAttribute;

		GameEntity Entity;

		[SetUp]
		public void SetUp()
		{
			this.Entity = new GameEntity("Entity");
			this.Component = new Mock<Component>("Component");
			this.Component.Setup(c => c.Init(this.Entity));

			this.RenderableComponent = new Mock<RenderableComponent>("RenderableComponent");
			this.RenderableComponent.Setup(c => c.Init(this.Entity));

			this.Attribute = new Mock<Attribute>("Attribute", 0.0);
			this.GenericAttribute = new Mock<Attribute<int>>("GenericAttribute", 1);

			this.Entity.AddComponent(this.Component.Object);
			this.Entity.AddComponent(this.RenderableComponent.Object);
			this.Entity.Attributes.Add(this.Attribute.Object);
			this.Entity.Attributes.Add(this.GenericAttribute.Object);
		}

		#region Adding
		[Test]
		public void DoComponentsAdd()
		{
			Assert.AreEqual(2, this.Entity.Components.Count);
			this.Component.Verify(c => c.Init(this.Entity));
			this.RenderableComponent.Verify(c => c.Init(this.Entity));
		}

		[Test]
		public void DoesAttributeAdd()
		{
			Assert.AreEqual(2, this.Entity.Attributes.Count);
		}

		[Test]
		public void ThrowsExceptionOnAddingSameComponentManyTimes()
		{
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Entity.AddComponent(this.Component.Object));
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Entity.AddComponent(this.RenderableComponent.Object));
		}

		[Test]
		public void ThrowsExceptionOnAddingSameAttributeManyTimes()
		{
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Entity.Attributes.Add(this.Attribute.Object));
		}

		[Test]
		public void GettingExistingAttributeWithGetOrCreateMethodsReturnsThisAttribute()
		{
			Assert.AreEqual(this.Attribute.Object, this.Entity.Attributes.GetOrCreate(this.Attribute.Object.Id));
			Assert.AreEqual(this.GenericAttribute.Object, this.Entity.Attributes.GetOrCreate<int>(this.GenericAttribute.Object.Id));
		}

		[Test]
		public void GettingExisitngAttributeWithIncorrectTypeThrowsException()
		{
			Assert.Throws<System.InvalidCastException>(() => this.Entity.Attributes.Get<float>(this.GenericAttribute.Object.Id));
			Assert.Throws<System.InvalidCastException>(() => this.Entity.Attributes.GetOrCreate<float>(this.GenericAttribute.Object.Id));
		}
		#endregion

		#region Getting data
		[Test]
		public void GettingExistingAttribute()
		{
			var attrib = this.Entity.Attributes[this.Attribute.Object.Id];
			Assert.AreEqual(attrib, this.Attribute.Object);
		}

		[Test]
		public void GettingNonExistionAttributeReturnsNull()
		{
			var attrib = this.Entity.Attributes["FancyAttribute"];
			Assert.IsNull(attrib);
		}

		#endregion

		#region Other
		[Test]
		public void DoComponentsUpdate([Values(0.0)] double delta)
		{
			this.Component.Setup(c => c.Update(delta));
			this.RenderableComponent.Setup(c => c.Update(delta));
			this.Entity.Update(delta);
			this.Component.VerifyAll();
			this.RenderableComponent.VerifyAll();
		}

		[Test]
		public void DoesComponentRender()
		{
			this.RenderableComponent.Setup(c => c.Render());
			this.Entity.Render();
			this.RenderableComponent.VerifyAll();
		}
		#endregion
	}
}
