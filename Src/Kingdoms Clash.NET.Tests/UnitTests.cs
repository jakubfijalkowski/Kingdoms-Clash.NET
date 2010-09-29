using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units;
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
			this.Description = new UnitDescription(100, 5f, 10f);
			this.Unit = new Unit(this.Description, null);
			this.Unit.Init(null);
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
	}
}
