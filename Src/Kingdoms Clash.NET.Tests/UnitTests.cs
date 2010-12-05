using ClashEngine.NET.EntitiesManager;
using Kingdoms_Clash.NET.Interfaces.Player;
using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units;
using Moq;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{
	[TestFixture(Description = "Testy dla klasy Unit")]
	public class UnitTests
	{
		private IUnitDescription Description;
		private IUnit Unit;
		private Mock<TestPlayer> Player;

		[SetUp]
		public void SetUp()
		{
			this.Player = new Mock<TestPlayer>();
			this.Player.SetupAllProperties();

			this.Description = new UnitDescription("sth", 100, 1f, 5f, 10f);

			this.Unit = new Unit(this.Description, this.Player.Object);
			this.Unit.OwnerManager = null;
			this.Unit.OnInit();
		}

		[Test]
		public void UnitHasSameAttributesAsDescription()
		{
			Assert.AreEqual(this.Description, this.Unit.Description);
			Assert.AreEqual(this.Description.Health, this.Unit.Health);
		}

		[Test]
		public void UnitHasSameComponentsAsDescription()
		{
			foreach (var component in this.Description.Components)
			{
				Assert.IsNotNull(this.Unit.Components.GetSingle(component.GetType()));
			}
		}

		public abstract class TestPlayer
			: GameEntity, IPlayer
		{
			public TestPlayer()
				: base("TestPlayer")
			{ }

			#region IPlayer Members
			public abstract string Name { get; set; }

			public abstract Interfaces.IGameState GameState { get; set; }

			public abstract INation Nation { get; set; }

			public abstract System.Collections.Generic.IList<IUnit> Units { get; set; }

			public abstract Interfaces.Resources.IResourcesCollection Resources { get; set; }

			public abstract int Health { get; set; }

			public abstract uint MaxHealth { get; set; }

			public abstract PlayerType Type { get; set; }

			public abstract event CollisionWithPlayerEventHandler Collide;
			#endregion
		}
	}
}
