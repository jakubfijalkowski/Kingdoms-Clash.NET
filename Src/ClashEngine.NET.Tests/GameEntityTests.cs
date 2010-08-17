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

			this.Entity.AddComponent(this.Component.Object);
			this.Entity.AddComponent(this.RenderableComponent.Object);
			this.Entity.AddAttribute(this.Attribute.Object);
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
			Assert.AreEqual(1, this.Entity.Attributes.Count);
			Assert.AreEqual(this.Entity.Attributes[0], this.Attribute.Object);
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
			Assert.Throws<Exceptions.ArgumentAlreadyExistsException>(() => this.Entity.AddAttribute(this.Attribute.Object));
		}
		#endregion

		#region Getting data
		[Test]
		public void GettingExistingAttribute()
		{
			var attrib = this.Entity.GetAttribute(this.Attribute.Object.Id);
			Assert.AreEqual(attrib, this.Attribute.Object);
		}

		[Test]
		public void GettingNonExistionAttributeReturnsNull()
		{
			var attrib = this.Entity.GetAttribute("FancyAttribute");
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
