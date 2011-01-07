using System.Collections.Generic;
using System.ComponentModel;
using ClashEngine.NET.Data.Internals;
using ClashEngine.NET.Extensions;
using NUnit.Framework;

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

		[Test]
		public void MultiParameterIndexer1()
		{
			var level = new IndexerLevel(typeof(DataClass2), "5, asd", 0, (i) => { });
			level.UpdateValue(new DataClass2());
			Assert.AreEqual("asd5", level.Value);
		}

		[Test]
		public void MultiParameterIndexer2()
		{
			var level = new IndexerLevel(typeof(DataClass2), "5,' asd'", 0, (i) => { });
			level.UpdateValue(new DataClass2());
			Assert.AreEqual(" asd5", level.Value);
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
					this.PropertyChanged.Raise(this, "Item." + index);
				}
			}

			#region INotifyPropertyChanged Members
			public event PropertyChangedEventHandler PropertyChanged;
			#endregion
		}

		private class DataClass2
		{
			public string this[int index, string text]
			{
				get { return text + index; }
				set
				{ }
			}
		}
	}
}
