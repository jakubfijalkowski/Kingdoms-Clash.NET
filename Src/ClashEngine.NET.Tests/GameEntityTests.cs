using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testowanie tworzenia encji i komponentów")]
	public class GameEntityTests
	{
		Mock<Component> Component = new Mock<Component>("MoqComponent");
		Mock<Attribute> Attribute = new Mock<Attribute>("MoqAttribute", 0.0);
		GameEntity Entity = new GameEntity("MoqEntity");

		[Test]
		public void ComponentAdds()
		{
			this.Entity.AddComponent(this.Component.Object);
			Assert.AreEqual(this.Entity.Components.Count, 1);
			Assert.AreEqual(this.Entity.Components[0], this.Component.Object);
		}

		[Test]
		public void MultipleSameComponentsAdd()
		{
			Assert.Throws<Exceptions.AlreadyExistsException>(() => this.Entity.AddComponent(this.Component.Object));
		}

		[Test]
		public void ComponentsUpdate()
		{
			this.Component.Setup((c) => c.Update(0.0));
			this.Entity.Update(0.0);
			this.Component.Verify((c) => c.Update(0.0));
		}

		[Test]
		public void AttributeAdds()
		{
			this.Entity.AddAttribute(this.Attribute.Object);
			Assert.AreEqual(this.Entity.Attributes.Count, 1);
			Assert.AreEqual(this.Entity.Attributes[0], this.Attribute.Object);
		}

		[Test]
		public void MultipleSameAttributesAdd()
		{
			Assert.Throws<Exceptions.AlreadyExistsException>(() => this.Entity.AddAttribute(this.Attribute.Object));
		}

		[Test]
		public void GettingExistingAttribute()
		{
			var attrib = this.Entity.GetAttribute(this.Attribute.Object.Id);
			Assert.AreEqual(attrib, this.Attribute.Object);
		}

		[Test]
		public void GettingNonExistionAttribute()
		{
			var attrib = this.Entity.GetAttribute("FancyAttribute");
			Assert.IsNull(attrib);
		}
	}
}
