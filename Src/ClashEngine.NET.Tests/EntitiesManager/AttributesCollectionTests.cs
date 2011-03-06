using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.EntitiesManager
{
	using NET.EntitiesManager;
	using NET.Interfaces.EntitiesManager;

	[TestFixture(Description = "Testy dla kolekcji atrybutów")]
	public class AttributesCollectionTests
	{
		private IAttributesCollection Collection;

		private Mock<Attribute> Attribute;
		private Mock<Attribute<int>> GenericAttribute;

		[SetUp]
		public void SetUp()
		{
			GameEntity ent = new GameEntity("Test");
			this.Collection = new AttributesCollection(ent);

			this.Attribute = new Mock<Attribute>("Attribute", 0.0);
			this.GenericAttribute = new Mock<Attribute<int>>("GenericAttribute", 1);

			this.Collection.Add(this.Attribute.Object);
			this.Collection.Add(this.GenericAttribute.Object);
		}

		[Test]
		public void DoesAttributeAdd()
		{
			Assert.AreEqual(2, this.Collection.Count);
		}

		[Test]
		public void ThrowsExceptionOnAddingSameAttributeManyTimes()
		{
			Assert.Throws<System.ArgumentException>(() => this.Collection.Add(this.Attribute.Object));
		}

		[Test]
		public void GettingExistingAttributeWithGetOrCreateMethodsReturnsThisAttribute()
		{
			Assert.AreEqual(this.Attribute.Object, this.Collection.GetOrCreate(this.Attribute.Object.Id));
			Assert.AreEqual(this.GenericAttribute.Object, this.Collection.GetOrCreate<int>(this.GenericAttribute.Object.Id));
		}

		[Test]
		public void GettingExisitngAttributeWithIncorrectTypeThrowsException()
		{
			Assert.Throws<System.InvalidCastException>(() => this.Collection.Get<float>(this.GenericAttribute.Object.Id));
			Assert.Throws<System.InvalidCastException>(() => this.Collection.GetOrCreate<float>(this.GenericAttribute.Object.Id));
		}

		[Test]
		public void GettingExistingAttribute()
		{
			var attrib = this.Collection[this.Attribute.Object.Id];
			Assert.AreEqual(attrib, this.Attribute.Object);
		}

		[Test]
		public void GettingNonExistionAttributeThrowsException()
		{
			Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => { var tmp = this.Collection["FancyAttribute"]; });
		}
	}
}
