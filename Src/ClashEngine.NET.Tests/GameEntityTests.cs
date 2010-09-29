using Moq;
using NUnit.Framework;

using ClashEngine.NET.EntitiesManager;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testowanie tworzenia encji i komponentów")]
	public class GameEntityTests
	{
		private Mock<Component> Component;
		private Mock<RenderableComponent> RenderableComponent;
		private Mock<Attribute> Attribute;
		private Mock<Attribute<int>> GenericAttribute;

		private GameEntity Entity;

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

			this.Entity.Components.Add(this.Component.Object);
			this.Entity.Components.Add(this.RenderableComponent.Object);
			this.Entity.Attributes.Add(this.Attribute.Object);
			this.Entity.Attributes.Add(this.GenericAttribute.Object);
		}

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
	}
}
