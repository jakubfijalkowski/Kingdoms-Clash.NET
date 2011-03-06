using System.ComponentModel;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.Data
{
	using NET.Data.Internals;
	using NET.Extensions;

	[TestFixture(Description = "Testy dla wewnętrznej klasy PropertyLevel")]
	public class PropertyLevelTests
	{
		private IPropertyLevel Level;
		private bool WasValueChangedCalled;
		private DataClass Data;

		[SetUp]
		public void SetUp()
		{
			this.Data = new DataClass();
			this.Data.Data = 10;
			this.WasValueChangedCalled = false;
			this.Level = new PropertyLevel(typeof(DataClass), "Data", 150, this.ValueChanged);
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
			Assert.AreEqual(this.Data.Data, this.Level.Value);
		}

		[Test]
		public void LevelValueSet()
		{
			this.Level.SetValue(this.Data, 150);
			Assert.AreEqual(150, this.Data.Data);
		}

		[Test]
		public void ValueChangedEventRaised()
		{
			this.Data.Data = 200;
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
			private int _Data;

			public int Data
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
	}
}
