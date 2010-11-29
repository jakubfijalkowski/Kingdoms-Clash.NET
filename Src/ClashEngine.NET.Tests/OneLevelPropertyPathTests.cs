using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ClashEngine.NET.Data;
using System.ComponentModel;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla OneLevelPropertyPath")]
	public class OneLevelPropertyPathTests
	{
		private OneLevelPropertyPath Path;
		private DataClass Data;

		[SetUp]
		public void SetUp()
		{
			this.Data = new DataClass { Value = 1 };

			this.Path = new OneLevelPropertyPath("Value", typeof(DataClass));
			this.Path.BeginInit();
			this.Path.Root = this.Data;
			this.Path.EndInit();
		}

		[TearDown]
		public void TearDown()
		{
			this.Path.Dispose();
		}

		[Test]
		public void TestTypes()
		{
			Assert.AreEqual(typeof(DataClass), this.Path.RootType);
			Assert.AreEqual(typeof(int), this.Path.ValueType);
			Assert.AreEqual(this.Data, this.Path.Root);
		}

		[Test]
		public void GetsCorrectData()
		{
			Assert.AreEqual(this.Data.Value, this.Path.Value);
		}

		[Test]
		public void SetsCorrectData()
		{
			this.Path.Value = 10;
			Assert.AreEqual(10, this.Data.Value);
		}

		[Test]
		public void EventInPathRaisesPropertyChangedLevel0()
		{
			bool called = false;
			PropertyChangedEventHandler @event = (o, e) =>
			{ called = true; };
			this.Path.PropertyChanged += @event;
			this.Data.Value = 200;
			Assert.True(called);
			Assert.AreEqual(200, this.Path.Value);
			this.Path.PropertyChanged -= @event;
		}

		[Test]
		public void ThrowsExceptionOnChangingRootType()
		{
			Assert.Throws<ArgumentException>(() => this.Path.Root = this);
		}

		#region Data class
		private class DataClass
			: INotifyPropertyChanged
		{
			private int _Value;

			public int Value
			{
				get { return this._Value; }
				set
				{
					this._Value = value;
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
					}
				}
			}

			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}
		#endregion
	}
}
