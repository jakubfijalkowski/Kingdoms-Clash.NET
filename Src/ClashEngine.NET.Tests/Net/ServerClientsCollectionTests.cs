using System;
using Moq;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.Net
{
	using Interfaces.Net;
	using NET.Net.Internals;

	[TestFixture(Description = "Testy dla ServerClientsCollectionTests")]
	public class ServerClientsCollectionTests
	{
		[Test]
		public void AddAndInsertThrowsException()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Assert.Throws<NotSupportedException>(() => collection.Add(null));
			Assert.Throws<NotSupportedException>(() => collection.Insert(0, null));
		}

		[Test]
		public void InternalAddWorks()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Mock<IClient> obj1 = new Mock<IClient>();
			Mock<IClient> obj2 = new Mock<IClient>();
			collection.InternalAdd(obj1.Object);
			Assert.AreEqual(1, collection.Count);
			collection.InternalAdd(obj2.Object);
			Assert.AreEqual(2, collection.Count);
		}

		[Test]
		public void RemoveAtClosesConnectionAndRemovesElementFromCollection()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Mock<IClient> obj1 = new Mock<IClient>();
			Mock<IClient> obj2 = new Mock<IClient>();
			collection.InternalAdd(obj1.Object);
			collection.InternalAdd(obj2.Object);

			obj1.Setup(c => c.Close(true));
			obj2.Setup(c => c.Close(true));
			collection.RemoveAt(0);
			obj1.Verify(c => c.Close(true), Times.Once());
			obj2.Verify(c => c.Close(true), Times.Never());
			Assert.AreEqual(1, collection.Count);
			Assert.AreEqual(obj2.Object, collection[0]);
		}

		[Test]
		public void RemoveClosesConnectionAndRemovesElementFromCollection()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Mock<IClient> obj1 = new Mock<IClient>();
			Mock<IClient> obj2 = new Mock<IClient>();
			collection.InternalAdd(obj1.Object);
			collection.InternalAdd(obj2.Object);

			obj1.Setup(c => c.Close(true));
			obj2.Setup(c => c.Close(true));
			collection.Remove(obj1.Object);
			obj1.Verify(c => c.Close(true), Times.Once());
			obj2.Verify(c => c.Close(true), Times.Never());
			Assert.AreEqual(1, collection.Count);
			Assert.AreEqual(obj2.Object, collection[0]);
		}

		[Test]
		public void IndexerWorks()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Mock<IClient> obj1 = new Mock<IClient>();
			Mock<IClient> obj2 = new Mock<IClient>();
			collection.InternalAdd(obj1.Object);
			collection.InternalAdd(obj2.Object);
			Assert.AreEqual(obj1.Object, collection[0]);
			Assert.AreEqual(obj2.Object, collection[1]);
		}

		[Test]
		public void ContainsWorks()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Mock<IClient> obj1 = new Mock<IClient>();
			Mock<IClient> obj2 = new Mock<IClient>();
			collection.InternalAdd(obj1.Object);
			Assert.IsTrue(collection.Contains(obj1.Object));
			Assert.IsFalse(collection.Contains(obj2.Object));
		}

		[Test]
		public void IndexOfWorks()
		{
			ServerClientsCollection collection = new ServerClientsCollection();
			Mock<IClient> obj1 = new Mock<IClient>();
			Mock<IClient> obj2 = new Mock<IClient>();
			collection.InternalAdd(obj1.Object);
			Assert.AreEqual(0, collection.IndexOf(obj1.Object));
			Assert.AreEqual(-1, collection.IndexOf(obj2.Object));
		}
	}
}
