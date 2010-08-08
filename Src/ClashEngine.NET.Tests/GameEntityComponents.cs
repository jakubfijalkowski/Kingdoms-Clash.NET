using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testowanie tworzenia encji i komponentów")]
	public class GameEntityComponents
	{
		Mock<Component> Component = new Mock<Component>("MoqComponent");
		GameEntity Entity = new GameEntity("MoqEntity");

		[Test]
		public void ComponentAdds()
		{
			this.Entity.AddComponent(this.Component.Object);
			Assert.AreEqual(this.Entity.Components.Count, 1);
			Assert.AreEqual(this.Entity.Components[0], this.Component.Object);
		}

		[Test]
		public void ComponentsUpdate()
		{
			this.Component.Setup((c) => c.Update(0.0));
			this.Entity.Update(0.0);
			this.Component.Verify((c) => c.Update(0.0));
		}
	}
}
