using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	using Interfaces;
using Moq;

	[TestFixture(Description = "Testuje manager callbacków.")]
	public class MainThreadCallbacksManagerTests
	{
		private IMainThreadCallbacksManager Manager;
		private Mock<IMockCallback> Callback1;
		private Mock<IMockCallback> Callback2;

		[SetUp]
		public void SetUp()
		{
			this.Manager = MainThreadCallbacksManager.Instance;

			this.Callback1 = new Mock<IMockCallback>();
			this.Callback1.Setup(c => c.Call()).Returns(false);

			this.Callback2 = new Mock<IMockCallback>();
			this.Callback2.Setup(c => c.Call()).Returns(true);

			this.Manager.Add(this.Callback1.Object.Call);
			this.Manager.Add(this.Callback2.Object.Call);
			this.Manager.Call();
		}

		[Test]
		public void Calls()
		{
			this.Callback1.Verify(c => c.Call());
			this.Callback2.Verify(c => c.Call());
		}

		[Test]
		public void SecondCallInvokeOnlyCallback1()
		{
			this.Manager.Call();
			this.Callback1.Verify(c => c.Call(), Times.Exactly(2));
			this.Callback2.Verify(c => c.Call(), Times.Once());
		}

		public interface IMockCallback
		{
			bool Call();
		}
	}
}
