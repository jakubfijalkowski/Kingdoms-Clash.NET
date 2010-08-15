using System;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy managera zasobów")]
	public class ResourcesManagerTests
	{
		private ResourcesManager.ResourcesManager Manager;
		private Mock<MockResource> Resource1;
		private Mock<MockResource> Resource2;

		[SetUp]
		public void SetUp()
		{
			this.Manager = ResourcesManager.ResourcesManager.Instance;

			this.Resource1 = new Mock<MockResource>();
			this.Resource1.Setup(r => r.Load());
			this.Resource1.Setup(r => r.Free());
			this.Manager.Load("resource 1", this.Resource1.Object);

			this.Resource2 = new Mock<MockResource>();
			this.Resource2.Setup(r => r.Load());
			this.Resource2.Setup(r => r.Free());
			this.Manager.Load("resource 2", this.Resource2.Object);
		}

		#region Loading/freeing
		[Test]
		public void DoResourcesLoad()
		{
			Assert.AreEqual(2, this.Manager.TotalCount);
		}

		[Test]
		public void DoesResourceFreeByResource()
		{
			int old = this.Manager.TotalCount;
			this.Manager.Free(this.Resource1.Object);
			Assert.AreEqual(old - 1, this.Manager.TotalCount);

			//Wracamy do stanu "sprzed" - można to zrobić lepiej?
			this.Manager.Load("resource 1", this.Resource1.Object);
		}
		#endregion

		#region Mock class
		/// <summary>
		/// Klasa zasobu do użycia przez Moq.
		/// </summary>
		private class MockResource
			: ResourcesManager.Resource
		{
			public override void Load()
			{
				throw new NotImplementedException();
			}

			public override void Free()
			{
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}
