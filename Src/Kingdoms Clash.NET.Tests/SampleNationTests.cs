using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units.Sample;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{
	[TestFixture(Description = "Testy dla SampleNation")]
	public class SampleNationTests
	{
		private SampleNation Nation;
		private IUnit Unit;

		[SetUp]
		public void SetUp()
		{
			this.Nation = new SampleNation();
			this.Unit = this.Nation.CreateUnit(this.Nation.AvailableUnits[0], null);
		}

		[Test]
		public void NationIsCreatingUnit()
		{
			Assert.AreNotEqual(null, this.Unit);
		}

		[Test]
		public void NationIsCreatingDemandUnit()
		{
			Assert.AreEqual(this.Nation.AvailableUnits[0], this.Unit.Description);
		}
	}
}
