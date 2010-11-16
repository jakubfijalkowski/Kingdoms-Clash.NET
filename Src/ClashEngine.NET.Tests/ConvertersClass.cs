using ClashEngine.NET.Converters;
using NUnit.Framework;
using OpenTK;

namespace ClashEngine.NET.Tests
{
	[TestFixture(Description = "Testy dla konwerterów")]
	public class ConvertersClass
	{
		private Vector2Converter V2 = new Vector2Converter();
		private Vector4Converter V4 = new Vector4Converter();

		#region Vector2
		[Test]
		public void Vector2FromString()
		{
			Assert.AreEqual(new Vector2(5, 10), V2.ConvertFrom("5,10"));
			Assert.AreEqual(new Vector2(4.12f, 10.88f), V2.ConvertFrom("4.12,10.88"));
		}

		[Test]
		public void Vector2ToString()
		{
			Assert.AreEqual("5,10", V2.ConvertTo(new Vector2(5, 10), typeof(string)));
			Assert.AreEqual("4.12,10.88", V2.ConvertTo(new Vector2(4.12f, 10.88f), typeof(string)));
		} 
		#endregion

		#region Vector4
		[Test]
		public void Vector4FromString()
		{
			Assert.AreEqual(new Vector4(5, 10, 15, 20), V4.ConvertFrom("5,10,15,20"));
			Assert.AreEqual(new Vector4(4.12f, 10.88f, 1.5f, 5.0f), V4.ConvertFrom("4.12,10.88,1.5,5"));
			Assert.AreEqual(new Vector4(4.12f, 10.88f, 1.5f, 1), V4.ConvertFrom("4.12,10.88,1.5"));
		}

		[Test]
		public void Vector4FromColorString()
		{
			Assert.AreEqual(new Vector4(1, 1, 1, 1), V4.ConvertFrom("white"));
			Assert.AreEqual(new Vector4(1, 0, 0, 1), V4.ConvertFrom("red"));
		}

		[Test]
		public void Vector4ToString()
		{
			Assert.AreEqual("5,10,15,20", V4.ConvertTo(new Vector4(5, 10, 15, 20), typeof(string)));
			Assert.AreEqual("4.12,10.88,1.5,5", V4.ConvertTo(new Vector4(4.12f, 10.88f, 1.5f, 5.0f), typeof(string)));
			Assert.AreEqual("4.12,10.88,1.5,1", V4.ConvertTo(new Vector4(4.12f, 10.88f, 1.5f, 1), typeof(string)));
		}
		#endregion
	}
}
