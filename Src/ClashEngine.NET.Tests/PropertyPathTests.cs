using System;
using System.ComponentModel;
using ClashEngine.NET.Data;
using ClashEngine.NET.Extensions;
using ClashEngine.NET.Interfaces.Data;
using NUnit.Framework;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla PropertyPath")]
	public class PropertyPathTests
		: INotifyPropertyChanged
	{
		private IPropertyPath Path;
		public Data1 Data;

		[SetUp]
		public void SetUp()
		{
			this.Data = new Data1();
			this.Data.Data = new Data2();
			this.Data.Data.Items = new Data3[]
			{
				new Data3{Value = 0},
				new Data3{Value = 1},
				new Data3{Value = 2},
				new Data3{Value = 3}
			};

			this.Path = new PropertyPath("Data.Data[1].Value", typeof(PropertyPathTests));
			this.Path.BeginInit();
			this.Path.Root = this;
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
			Assert.AreEqual(typeof(PropertyPathTests), this.Path.RootType);
			Assert.AreEqual(typeof(int), this.Path.ValueType);
			Assert.AreEqual(this, this.Path.Root);
		}

		[Test]
		public void GetsCorrectData()
		{
			Assert.AreEqual(this.Data.Data[1].Value, this.Path.Value);
		}

		[Test]
		public void SetsCorrectData()
		{
			this.Path.Value = 10;
			Assert.AreEqual(10, this.Data.Data[1].Value);
		}

		[Test]
		public void EventInPathRaisesPropertyChangedLevel3()
		{
			this.EventHelper(() => this.Data.Data[1].Value = 150, 150);
		}

		[Test]
		public void EventInPathRaisesPropertyChangedLevel25()
		{
			this.EventHelper(() => this.Data.Data[1] = new Data3 { Value = 200 }, 200);
		}

		[Test]
		public void EventInPathRaisesPropertyChangedLevel2()
		{
			this.EventHelper(() => this.Data.Data.Items = new Data3[] { new Data3 { Value = 10 }, new Data3 { Value = 250 } }, 250);
		}

		[Test]
		public void EventInPathRaisesPropertyChangedLevel1()
		{
			Data2 newData = new Data2();
			newData.Items = new Data3[]
			{
				new Data3{Value = 300},
				new Data3{Value = 400},
				new Data3{Value = 500}
			};

			this.EventHelper(() => this.Data.Data = newData, 400);
		}

		[Test]
		public void EventInPathRaisesPropertyChangedLevel0()
		{
			Data2 newData = new Data2();
			newData.Items = new Data3[]
			{
				new Data3{Value = 300},
				new Data3{Value = 400},
				new Data3{Value = 500}
			};

			this.EventHelper(() =>
			{
				this.Data = new Data1 { Data = newData };
				this.PropertyChanged(this, new PropertyChangedEventArgs("Data"));
			}, 400);
		}

		[Test]
		public void ThrowsExceptionOnChangingRootType()
		{
			Assert.Throws<ArgumentException>(() => this.Path.Root = this.Data);
		}

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Privates
		private void EventHelper(Action changeData, int expected)
		{
			bool called = false;
			PropertyChangedEventHandler @event = (o, e) =>
			{ called = true; };
			this.Path.PropertyChanged += @event;
			changeData();
			Assert.True(called);
			Assert.AreEqual(expected, this.Path.Value);
			this.Path.PropertyChanged -= @event;
		}
		#endregion

		#region Data classes
		public class Data1
			: INotifyPropertyChanged
		{
			private Data2 _Data;

			public Data2 Data
			{
				get { return this._Data; }
				set
				{
					this._Data = value;
					this.PropertyChanged.Raise(this, () => Data);
				}
			}

			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}

		public class Data2
			: INotifyPropertyChanged
		{
			private Data3[] _Items;
			public Data3[] Items
			{
				get { return this._Items; }
				set
				{
					this._Items = value;
				}
			}

			public Data3 this[int index]
			{
				get { return this.Items[index]; }
				set
				{
					this.Items[index] = value;
					this.PropertyChanged.Raise(this, "Item." + index);
				}
			}

			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}

		public class Data3
			: INotifyPropertyChanged
		{
			private int _Value;

			public int Value
			{
				get { return this._Value; }
				set
				{
					this._Value = value;
					this.PropertyChanged.Raise(this, () => Value);
				}
			}


			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}
		#endregion
	}
}
