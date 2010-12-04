using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units;
using Moq;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{
	[TestFixture(Description = "Testy dla klasy Nation i kolekcji opisów jednostek")]
	public class NationTests
	{
		private INation Nation;
		private IUnitDescription Description1;
		private IUnitDescription Description2;

		private Mock<UnitTests.TestPlayer> Player;

		[SetUp]
		public void SetUp()
		{
			this.Player = new Mock<UnitTests.TestPlayer>();
			this.Player.SetupAllProperties();

			this.Description1 = new UnitDescription("Unit1", 100, 1f, 5f, 5f);
			this.Description2 = new UnitDescription("Unit2", 100, 1f, 5f, 5f);

			this.Nation = new Nation("TestNation", "NonExisting", new IUnitDescription[] { this.Description1, this.Description2 });
		}

		[Test]
		public void DescriptionsWereAddedToNation()
		{
			Assert.AreEqual(2, this.Nation.AvailableUnits.Count);
			CollectionAssert.AreEquivalent(new IUnitDescription[] { this.Description1, this.Description2 }, this.Nation.AvailableUnits);
		}

		[Test]
		public void NationsReturnsCorrectUnit1()
		{
			var unit = this.Nation.AvailableUnits["Unit1"];
			Assert.IsNotNull(unit);
			Assert.AreEqual(this.Description1, unit);
		}

		[Test]
		public void NationsReturnsCorrectUnit2()
		{
			var unit = this.Nation.AvailableUnits["Unit2"];
			Assert.IsNotNull(unit);
			Assert.AreEqual(this.Description2, unit);
		}

		[Test]
		public void GettingUnitDescriptionWithWrongIdReturnsNull()
		{
			var unit = this.Nation.AvailableUnits["MyFancyUnit"];
			Assert.IsNull(unit);
		}
	}
}
