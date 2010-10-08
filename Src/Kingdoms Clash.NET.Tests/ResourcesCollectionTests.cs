using System.Collections.Generic;
using Kingdoms_Clash.NET.Interfaces.Resources;
using Kingdoms_Clash.NET.Resources;
using NUnit.Framework;

namespace Kingdoms_Clash.NET.Tests
{
	[TestFixture(Description = "Testy dla kolekcji zasobów.")]
	public class ResourcesCollectionTests
	{
		private IResourcesCollection Resources;

		[SetUp]
		public void SetUp()
		{
			this.Resources = new ResourcesCollection();

			this.Resources.Add("1", 10);
			this.Resources.Add("2", 10);
			this.Resources.Add("3", 10);
		}

		#region Adding/removing
		[Test]
		public void ResourcesWereAdded()
		{
			CollectionAssert.AreEquivalent(new KeyValuePair<string, uint>[]
			{
				new KeyValuePair<string, uint>("1", 10),
				new KeyValuePair<string, uint>("2", 10),
				new KeyValuePair<string, uint>("3", 10)
			}, this.Resources);
		}

		[Test]
		public void AddingExistingResourceIncreaseCurrentStorage()
		{
			this.Resources.Add("1", 20);
			Assert.AreEqual(30, this.Resources["1"]);
		}

		[Test]
		public void RemovingExisitngResourceDecreasesCurrentStorage()
		{
			this.Resources.Remove("1", 5);
			Assert.AreEqual(5, this.Resources["1"]);
		}
		#endregion

		#region Getting data
		[Test]
		public void GettingExistingResourceReturnsCorrectValue()
		{
			Assert.AreEqual(10, this.Resources["1"]);
			Assert.AreEqual(10, this.Resources["2"]);
			Assert.AreEqual(10, this.Resources["3"]);
		}

		[Test]
		public void GettingNonExisitngResourceReturns0()
		{
			Assert.AreEqual(0, this.Resources["FancyResource"]);
			uint val;
			Assert.True(this.Resources.TryGetValue("FancyResource", out val));
			Assert.AreEqual(0, val);
		}

		[Test]
		public void UsingContainChecksCurrentStorage()
		{
			Assert.True(this.Resources.Contains("1", 5));
			Assert.False(this.Resources.Contains("1", 15));
			Assert.False(this.Resources.Contains("FancyResource", 15));
		}
		#endregion
	}
}
