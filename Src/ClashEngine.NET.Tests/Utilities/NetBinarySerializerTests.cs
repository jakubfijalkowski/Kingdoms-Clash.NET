using System;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.Utilities
{
	using NET.Utilities;

	[TestFixture]
	public class NetBinarySerializerTests
	{
		#region Datas
		private static readonly object[] SimpleData = new object[] { (int)1, (short)2, null, (byte)5 };
		private static readonly byte[] SimpleDataSerialized = new byte[]
		{
			0x01, 0x00, 0x00, 0x00, //1, 0-3
			0x02, 0x00,             //2, 4-5
			0x00,                   //3, 6
			0x05                    //4, 7
		};

		private static readonly object[] Data = new object[] { true, null, (byte)1, 'b', DBNull.Value, (decimal)1, (double)1, (Int16)(-5), (Int32)(-6), (Int64)7,
				(SByte)(-2), (Single)1, (UInt16)9, (UInt32)10, (UInt64)11 };
		private static readonly byte[] DataSerialized = new byte[]
		{	 
			0x01,                                           //1, 0
			0x00,                                           //2, 1
			0x01,                                           //3, 2
			0x62, 0x00,                                     //4, 3-4
			0x00,                                           //5, 5
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, //6, 6-13
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, //7, 13-21
			0xFB, 0xFF,                                     //8, 22-23
			0xFA, 0xFF, 0xFF, 0xFF,                         //9, 24-27
			0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //10, 28-35
			0xFE,                                           //11, 36
			0x00, 0x00, 0x80, 0x3F,                         //12, 37-40
			0x09, 0x00,                                     //13, 41-42
			0x0A, 0x00, 0x00, 0x00,                         //14, 43-46
			0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00  //15, 47-54
		};

		private static readonly string Text = "Zażółcić gęślą jaźń";
		private static readonly byte[] TextSerialized;

		/// <summary>
		/// Taka customowa inicjalizacja jest o wiele prostsza ;)
		/// </summary>
		static NetBinarySerializerTests()
		{
			TextSerialized = new byte[2 + Text.Length * 2];
			TextSerialized[0] = (byte)Text.Length;
			TextSerialized[1] = 0x00;
			System.Text.Encoding.Unicode.GetBytes(Text, 0, Text.Length, TextSerialized, 2);
		}
		#endregion

		[Test]
		public void SimpleSerializationList()
		{
			byte[] output = NetBinarySerializer.Serialize(SimpleData);
			CollectionAssert.AreEqual(SimpleDataSerialized, output);
		}

		[Test]
		public void SimpleSerialization()
		{
			byte[] output = new byte[SimpleDataSerialized.Length];
			NetBinarySerializer.Serialize(output, SimpleData);
			CollectionAssert.AreEqual(SimpleDataSerialized, output);
		}

		[Test]
		public void SerializationList()
		{
			byte[] output = NetBinarySerializer.Serialize(Data);
			CollectionAssert.AreEqual(DataSerialized, output);
		}

		[Test]
		public void Serialization()
		{
			byte[] output = new byte[DataSerialized.Length];
			NetBinarySerializer.Serialize(output, Data);
			CollectionAssert.AreEqual(DataSerialized, output);
		}

		[Test]
		public void StringSerializationList()
		{
			byte[] output = NetBinarySerializer.Serialize(Text);
			CollectionAssert.AreEqual(TextSerialized, output);
		}

		[Test]
		public void StringSerialization()
		{
			byte[] output = new byte[TextSerialized.Length];
			NetBinarySerializer.Serialize(output, Text);
			CollectionAssert.AreEqual(TextSerialized, output);
		}
	}
}
