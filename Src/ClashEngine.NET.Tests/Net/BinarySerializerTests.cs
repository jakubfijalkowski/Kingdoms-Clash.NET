using System;
using NUnit.Framework;

namespace ClashEngine.NET.Tests.Net
{
	using NET.Net;

	[TestFixture]
	public class BinarySerializerTests
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
		static BinarySerializerTests()
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
			byte[] output = BinarySerializer.StaticSerialize(SimpleData);
			CollectionAssert.AreEqual(SimpleDataSerialized, output);
		}

		[Test]
		public void SimpleSerialization()
		{
			byte[] output = new byte[SimpleDataSerialized.Length];
			BinarySerializer.StaticSerialize(output, SimpleData);
			CollectionAssert.AreEqual(SimpleDataSerialized, output);
		}

		[Test]
		public void SerializationList()
		{
			byte[] output = BinarySerializer.StaticSerialize(Data);
			CollectionAssert.AreEqual(DataSerialized, output);
		}

		[Test]
		public void Serialization()
		{
			byte[] output = new byte[DataSerialized.Length];
			BinarySerializer.StaticSerialize(output, Data);
			CollectionAssert.AreEqual(DataSerialized, output);
		}

		[Test]
		public void StringSerializationList()
		{
			byte[] output = BinarySerializer.StaticSerialize(Text);
			CollectionAssert.AreEqual(TextSerialized, output);
		}

		[Test]
		public void StringSerialization()
		{
			byte[] output = new byte[TextSerialized.Length];
			BinarySerializer.StaticSerialize(output, Text);
			CollectionAssert.AreEqual(TextSerialized, output);
		}

		[Test]
		public void Deserialization()
		{
			var serializer = new BinarySerializer(DataSerialized);
			Assert.AreEqual(true, serializer.GetBool());
			Assert.AreEqual((byte)0, serializer.GetByte());
			Assert.AreEqual((byte)1, serializer.GetByte());
			Assert.AreEqual('b', serializer.GetChar());
			Assert.AreEqual((byte)0, serializer.GetByte());
			Assert.AreEqual(1d, serializer.GetDouble());
			Assert.AreEqual(1d, serializer.GetDouble());
			Assert.AreEqual((short)-5, serializer.GetInt16());
			Assert.AreEqual(-6, serializer.GetInt32());
			Assert.AreEqual(7L, serializer.GetInt64());
			Assert.AreEqual((sbyte)-2, serializer.GetSByte());
			Assert.AreEqual(1f, serializer.GetFloat());
			Assert.AreEqual((ushort)9, serializer.GetUInt16());
			Assert.AreEqual(10U, serializer.GetUInt32());
			Assert.AreEqual(11UL, serializer.GetUInt64());
		}

		[Test]
		public void StringDeserialization()
		{
			var serializer = new BinarySerializer(TextSerialized);
			Assert.AreEqual(Text, serializer.GetString());
		}
	}
}
