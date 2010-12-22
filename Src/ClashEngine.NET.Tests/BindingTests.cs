using System.ComponentModel;
using ClashEngine.NET.Data;
using ClashEngine.NET.Extensions;
using ClashEngine.NET.Interfaces.Data;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla Binding")]
	public class BindingTests
	{
		private TestData Data1;
		private TestData Data2;

		[SetUp]
		public void SetUp()
		{
			this.Data1 = new TestData();
			this.Data2 = new TestData();
			this.Data1.Data = 1;
			this.Data2.Data = 2;
		}

		[Test]
		public void OneTimeBindingTests()
		{
			this.Data1.Data = 88;
			var binding = new Binding(this.Data1, new PropertyPath("Data"), this.Data2, new PropertyPath("Data"), BindingMode.OneTime);
			Assert.AreEqual(this.Data1.Data, this.Data2.Data);
			this.Data1.Data = 100;
			Assert.AreNotEqual(this.Data1.Data, this.Data2.Data);
			this.Data2.Data = 150;
			Assert.AreNotEqual(this.Data1.Data, this.Data2.Data);
			binding.Dispose();
		}

		[Test]
		public void OneWayBindingTests()
		{
			var binding = new Binding(this.Data1, new PropertyPath("Data"), this.Data2, new PropertyPath("Data"), BindingMode.OneWay);
			this.Data1.Data = 88;
			Assert.AreEqual(this.Data1.Data, this.Data2.Data);
			this.Data1.Data = 100;
			Assert.AreEqual(this.Data1.Data, this.Data2.Data);
			this.Data2.Data = 150;
			Assert.AreNotEqual(this.Data1.Data, this.Data2.Data);
			binding.Dispose();
		}

		[Test]
		public void TwoWayBindingTests()
		{
			var binding = new Binding(this.Data1, new PropertyPath("Data"), this.Data2, new PropertyPath("Data"), BindingMode.TwoWay);
			this.Data1.Data = 88;
			Assert.AreEqual(this.Data1.Data, this.Data2.Data);
			this.Data1.Data = 100;
			Assert.AreEqual(this.Data1.Data, this.Data2.Data);
			this.Data2.Data = 150;
			Assert.AreEqual(this.Data1.Data, this.Data2.Data);
			binding.Dispose();
		}

		private class TestData
			: INotifyPropertyChanged
		{
			private int _Data;

			public int Data
			{
				get { return _Data; }
				set
				{
					_Data = value;
					this.PropertyChanged.Raise(this, () => Data);
				}
			}

			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}
	}
}
