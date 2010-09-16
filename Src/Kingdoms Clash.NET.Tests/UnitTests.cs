using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units;
using Kingdoms_Clash.NET.Units.Sample;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{

	[TestFixture(Description = "Testy dla klasy Unit")]
	public class UnitTests
	{
		private IUnitDescription Description;
		private IUnit Unit;

		[SetUp]
		public void SetUp()
		{
			this.Description = new SampleWorkerDescription();
			this.Unit = new Unit(this.Description, null);
			this.Unit.Init(null);
		}

		[Test]
		public void WorkerHasSameAttributesAsDescription()
		{
			Assert.AreEqual(this.Description, this.Unit.Description);
			Assert.AreEqual(this.Description.Health, this.Unit.Health);
		}
	}
}
