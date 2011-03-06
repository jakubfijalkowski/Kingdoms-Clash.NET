using NUnit.Framework;
using System;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla klasy MessagesCollection")]
	public class MessagesCollectionTests
	{
		[Test]
		public void AddAndInsertThrowsException()
		{
			var messages = new Net.Internals.MessagesCollection();
			Assert.Throws<NotSupportedException>(() => messages.Add(new Interfaces.Net.Message()));
			Assert.Throws<NotSupportedException>(() => messages.Insert(0, new Interfaces.Net.Message()));
		}

		[Test]
		public void InteralAddWorks()
		{
			var messages = new Net.Internals.MessagesCollection();
			messages.InternalAdd(new Interfaces.Net.Message());
			Assert.AreEqual(1, messages.Count);
		}

		[Test]
		public void RemoveAtWorks()
		{
			var messages = new Net.Internals.MessagesCollection();
			messages.InternalAdd(new Interfaces.Net.Message());
			messages.InternalAdd(new Interfaces.Net.Message());
			messages.RemoveAt(0);
			Assert.AreEqual(1, messages.Count);
		}

		[Test]
		public void RemoveWorks()
		{
			var messages = new Net.Internals.MessagesCollection();
			var msg = new Interfaces.Net.Message();
			messages.InternalAdd(msg);
			messages.InternalAdd(new Interfaces.Net.Message());
			messages.Remove(msg);
			Assert.AreEqual(1, messages.Count);
		}

		[Test]
		public void ContainsWorks()
		{
			var messages = new Net.Internals.MessagesCollection();
			var msg = new Interfaces.Net.Message(Interfaces.Net.MessageType.AllOk, null);
			messages.InternalAdd(msg);
			Assert.AreEqual(1, messages.Count);
			Assert.IsTrue(messages.Contains(msg));
			Assert.IsTrue(messages.Contains(Interfaces.Net.MessageType.AllOk));
			Assert.IsFalse(messages.Contains(Interfaces.Net.MessageType.Close));
		}

		[Test]
		public void IndexOfWorks()
		{
			var messages = new Net.Internals.MessagesCollection();
			var msg = new Interfaces.Net.Message(Interfaces.Net.MessageType.AllOk, null);
			messages.InternalAdd(new Interfaces.Net.Message(Interfaces.Net.MessageType.InvalidSequence, null));
			messages.InternalAdd(msg);
			Assert.AreEqual(2, messages.Count);
			Assert.AreEqual(1, messages.IndexOf(msg));
			Assert.AreEqual(1, messages.IndexOf(Interfaces.Net.MessageType.AllOk));
			Assert.AreEqual(0, messages.IndexOf(Interfaces.Net.MessageType.InvalidSequence));
			Assert.AreEqual(-1, messages.IndexOf(Interfaces.Net.MessageType.MessageEnd));
			Assert.AreEqual(0, 1);
		}
	}
}
