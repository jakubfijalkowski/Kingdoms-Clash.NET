using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ClashEngine.NET.Data.Internals;
using System.ComponentModel;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla wewnętrznej klasy PropertyLevel")]
	public class IndexerLevelTests
	{
		private IPropertyLevel Level;
		private bool WasValueChangedCalled;
		private DataClass Data;

		[SetUp]
		public void SetUp()
		{
			this.Data = new DataClass();
			this.Data.DataList.Add(1);
			this.Data.DataList.Add(2);
			this.Data.DataList.Add(3);
			this.Data.DataList.Add(4);

			this.WasValueChangedCalled = false;
			this.Level = new IndexerLevel(typeof(DataClass), "0", 150, this.ValueChanged);
			this.Level.RegisterPropertyChanged(this.Data);
		}

		[TearDown]
		public void TearDown()
		{
			this.Level.UnregisterPropertyChanged(this.Data);
		}

		[Test]
		public void LevelValueUpdated()
		{
			this.Level.UpdateValue(this.Data);
			Assert.AreEqual(this.Data[0], this.Level.Value);
		}

		[Test]
		public void LevelValueSet()
		{
			this.Level.SetValue(this.Data, 150);
			Assert.AreEqual(150, this.Data[0]);
		}

		[Test]
		public void ValueChangedEventRaised()
		{
			this.Data[0] = 200;
			Assert.True(this.WasValueChangedCalled);
		}

		private void ValueChanged(int lvl)
		{
			Assert.AreEqual(150, lvl);
			this.WasValueChangedCalled = true;
		}

		private class DataClass
			: INotifyPropertyChanged
		{
			public List<int> DataList = new List<int>();

			public int this[int index]
			{
				get { return this.DataList[index]; }
				set
				{
					this.DataList[index] = value;
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs("Item." + index));
					}
				}
			}

			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}
	}
}
