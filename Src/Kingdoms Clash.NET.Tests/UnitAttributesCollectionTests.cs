using NUnit.Framework;
using Kingdoms_Clash.NET.Units;

namespace Kingdoms_Clash.NET.Tests
{
	[TestFixture(Description = "Testy dla UnitAttributesCollection")]
	public class UnitAttributesCollectionTests
	{
		private UnitAttributesCollection Attributes;
		private UnitAttribute Attribute1;
		private UnitAttribute<float> Attribute2;

		[SetUp]
		public void SetUp()
		{
			this.Attributes = new UnitAttributesCollection();
			this.Attribute1 = new UnitAttribute("Attribute1", 5);
			this.Attribute2 = new UnitAttribute<float>("Attribute2", 10f);

			this.Attributes.Add(this.Attribute1);
			this.Attributes.Add(this.Attribute2);
		}

		[Test]
		public void AttributesAddedCorrectly()
		{
			Assert.AreEqual(2, this.Attributes.Count);
		}

		[Test]
		public void AddingAttributeWithSameIdThrowsException()
		{
			Assert.Throws<ClashEngine.NET.Exceptions.ArgumentAlreadyExistsException>(() => this.Attributes.Add(new UnitAttribute("Attribute1", 0)));
		}

		[Test]
		public void GettingAttributeNonGeneric()
		{
			Assert.AreEqual(5, this.Attributes["Attribute1"]);
			Assert.AreEqual(5, this.Attributes.Get("Attribute1"));
		}

		[Test]
		public void GettingAttributeGeneric()
		{
			Assert.AreEqual(10f, this.Attributes["Attribute2"]);
			Assert.AreEqual(10f, this.Attributes.Get("Attribute2"));
		}

		[Test]
		public void GettingAttributeAndCasting()
		{
			Assert.AreEqual(5, this.Attributes.Get<int>("Attribute1"));
			Assert.AreEqual(10f, this.Attributes.Get<float>("Attribute2"));
		}
	}
}
