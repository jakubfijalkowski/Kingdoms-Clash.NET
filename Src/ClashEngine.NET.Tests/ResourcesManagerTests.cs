using System;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	using Interfaces.ResourcesManager;

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

		[TearDown]
		public void TearDown()
		{
			this.Manager.Free(this.Resource1.Object);
			this.Manager.Free(this.Resource2.Object);
		}

		#region Loading/freeing
		[Test(Description = "Sprawdza dodawanie przez metodę niegeneryczną, metody generycznej nie da się użyć z Moq")]
		public void DoResourcesLoad()
		{
			Assert.AreEqual(2, this.Manager.TotalCount);
			Assert.AreEqual(this.Manager, this.Resource1.Object.Manager);
			Assert.AreEqual(this.Manager, this.Resource2.Object.Manager);
			this.Resource1.Verify(r => r.Load());
			this.Resource2.Verify(r => r.Load());
		}

		[Test(Description = "Sprawdza dodawanie przez metodę generyczną bez użycia Moq, wymaga testu DoesResourceFreeByResource")]
		public void DoResourcesLoadNonMoq()
		{
			int old = this.Manager.TotalCount;
			var res = this.Manager.Load<MockResource>("resource 3");
			Assert.AreEqual(old + 1, this.Manager.TotalCount);
		}
		
		[Test]
		public void LoadingExistingResourceReturnsExistingObject()
		{
			Assert.AreEqual(this.Resource1.Object, this.Manager.Load("resource 1", new MockResource())); //Wersja niegeneryczna
			Assert.AreEqual(this.Resource1.Object, this.Manager.Load<MockResource>("resource 1")); //Wersja generyczna.
		}

		[Test]
		public void LoadingExistingResourceUsingInvalidTypeThrowsException()
		{
			Assert.Throws<InvalidCastException>(() => this.Manager.Load<MockResource2>("resource 1"));
		}

		[Test]
		public void LoadingResourcesWithEmptyOrNullStringThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Load(null, new MockResource()));
			Assert.Throws<ArgumentNullException>(() => this.Manager.Load(string.Empty, new MockResource())); //Wersja niegeneryczna

			Assert.Throws<ArgumentNullException>(() => this.Manager.Load<MockResource>(null));
			Assert.Throws<ArgumentNullException>(() => this.Manager.Load<MockResource>(string.Empty)); //Wersja generyczna
		}

		[Test]
		public void DoesResourceFreeByResource()
		{
			int old = this.Manager.TotalCount;
			this.Manager.Free(this.Resource1.Object);
			Assert.AreEqual(old - 1, this.Manager.TotalCount);
			this.Resource1.Verify(r => r.Free());

			//Wracamy do stanu "sprzed" - można to zrobić lepiej?
			this.Manager.Load("resource 1", this.Resource1.Object);
		}

		[Test]
		public void DoesResourceFreeById()
		{
			int old = this.Manager.TotalCount;
			this.Manager.Free(this.Resource1.Object.Id);
			Assert.AreEqual(old - 1, this.Manager.TotalCount);
			this.Resource1.Verify(r => r.Free());

			//Wracamy do stanu "sprzed" - można to zrobić lepiej?
			this.Manager.Load("resource 1", this.Resource1.Object);
		}

		[Test]
		public void FreeingResourcesWithInvalidParameterThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Free((string)null));
			Assert.Throws<ArgumentNullException>(() => this.Manager.Free(string.Empty));
			Assert.Throws<ArgumentNullException>(() => this.Manager.Free((IResource)null));
		}

		[Test]
		public void FreeingNonExistingResourcesThrowsException()
		{
			Assert.Throws<Exceptions.NotFoundException>(() => this.Manager.Free("invalid resource"));
		}

		[Test]
		public void FreeingNonLoadedResourcesThrowsException()
		{
			Assert.Throws<ArgumentNullException>(() => this.Manager.Free(new MockResource2()));
		}
		#endregion

		#region Mock classes
		/// <summary>
		/// Klasa zasobu nie-abstrakcyjna(ale pusta) do użycia przez Moq #1.
		/// </summary>
		public class MockResource
			: IResource
		{
			#region IResource Members
			public string Id { get; set; }
			public string FileName { get; set; }
			public IResourcesManager Manager { get; set; }

			public virtual ResourceLoadingState Load()
			{
				return ResourceLoadingState.Success;
			}

			public virtual void Free()
			{ }
			#endregion

			#region IDisposable Members
			public void Dispose()
			{ }
			#endregion
		}

		/// <summary>
		/// Klasa zasobu nie-abstrakcyjna(ale pusta) do użycia przez Moq #2.
		/// </summary>
		public class MockResource2
			: IResource
		{
			#region IResource Members
			public string Id { get; set; }
			public string FileName { get; set; }
			public IResourcesManager Manager { get; set; }

			public virtual ResourceLoadingState Load()
			{
				return ResourceLoadingState.Success;
			}

			public virtual void Free()
			{ }
			#endregion

			#region IDisposable Members
			public void Dispose()
			{ }
			#endregion
		}
		#endregion
	}
}
