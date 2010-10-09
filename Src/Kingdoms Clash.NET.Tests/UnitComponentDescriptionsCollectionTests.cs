using ClashEngine.NET.Exceptions;
using Kingdoms_Clash.NET.Interfaces.Units;
using Kingdoms_Clash.NET.Units;
using Moq;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{
	[TestFixture(Description = "Testy dla kolekcji opisó komponentów")]
	public class UnitComponentDescriptionsCollectionTests
	{
		private IUnitComponentDescriptionsCollection Collection;

		private Mock<IUnitComponentDescription> Description1;
		private Mock<TestDescription> Description2;
		private Mock<TestDescription2> Description3;

		[SetUp]
		public void SetUp()
		{
			this.Collection = new UnitComponentDescriptionsCollection();

			this.Description1 = new Mock<IUnitComponentDescription>();
			this.Description2 = new Mock<TestDescription>();
			this.Description3 = new Mock<TestDescription2>();

			this.Collection.Add(this.Description1.Object);
			this.Collection.Add(this.Description2.Object);
			this.Collection.Add(this.Description3.Object);
		}

		#region Adding/Removing
		[Test]
		public void DescriptionsAdd()
		{
			Assert.AreEqual(3, this.Collection.Count);
		}

		[Test]
		public void AddingExistingDescThrowsException()
		{
			Assert.Throws<ArgumentAlreadyExistsException>(() => this.Collection.Add(this.Description1.Object));
		}

		public void RemovingRemovesDescription()
		{
			int old = this.Collection.Count;
			this.Collection.Remove(this.Description1.Object);
			Assert.AreEqual(old - 1, this.Collection.Count);
		}
		#endregion

		#region Getting
		[Test]
		public void ContainsWorks()
		{
			Assert.True(this.Collection.Contains(this.Description1.Object));
		}

		[Test]
		public void GettingSingleNonGenericReturnsCorrectObject()
		{
			Assert.AreEqual(this.Description1.Object, this.Collection.GetSingle(this.Description1.Object.GetType()));
		}

		[Test]
		public void GettingCollectionNonGenericReturnsCorrectCollection()
		{
			CollectionAssert.AreEquivalent(new IUnitComponentDescription[]
			{
				this.Description1.Object
			}, this.Collection.Get(this.Description1.Object.GetType()));

			CollectionAssert.AreEquivalent(new IUnitComponentDescription[]
			{
				this.Description1.Object,
				this.Description2.Object,
				this.Description3.Object
			}, this.Collection.Get(typeof(IUnitComponentDescription)));
		}

		[Test]
		public void GettingCollectionGenericReturnsCorrectCollection()
		{
			CollectionAssert.AreEquivalent(new IUnitComponentDescription[]
			{
				this.Description2.Object
			}, this.Collection.Get<TestDescription>());

			CollectionAssert.AreEquivalent(new IUnitComponentDescription[]
			{
				this.Description1.Object,
				this.Description2.Object,
				this.Description3.Object
			}, this.Collection.Get<IUnitComponentDescription>());
		}
		#endregion

		public class TestDescription
			: IUnitComponentDescription
		{
			#region IUnitComponentDescription Members
			public IUnitComponent Create()
			{
				return null;
			}
			#endregion
		}

		public class TestDescription2
			: IUnitComponentDescription
		{
			#region IUnitComponentDescription Members
			public IUnitComponent Create()
			{
				return null;
			}
			#endregion
		}
	}
}
