using System;
using System.Collections.Generic;
using System.Text;

namespace ClashEngine.NET.Net
{
	using Interfaces.Net;

	/// <summary>
	/// Helper przy serializowaniu danych binarnych do użytku sieciowego.
	/// Obsługuje wszystkie obiekty, które posiadają <see cref="System.TypeCode"/> z wyłączeniem
	/// <see cref="System.TypeCode.Object"/> oraz <see cref="System.TypeCode.DateTime"/>.
	/// </summary>
	/// <remarks>
	/// Serializer nie sprawdza poprawności danych.
	/// Do serializacji należy wywoływać metody statyczne.
	/// Nie jest thread-safe!
	/// </remarks>
	public class BinarySerializer
		: IBinarySerializer
	{
		#region Statics
		/// <summary>
		/// Serializuje obiekty do nowej tablicy.
		/// </summary>
		/// <param name="objs">Obiekty.</param>
		/// <returns></returns>
		public static byte[] StaticSerialize(params object[] objs)
		{
			List<byte> outputList = new List<byte>();
			foreach (var obj in objs)
			{
				if (obj == null)
					outputList.Add(0);
				else if (obj is IConvertible)
				{
					switch ((obj as IConvertible).GetTypeCode())
					{
						case TypeCode.Boolean:
							outputList.Add(((bool)obj ? (byte)1 : (byte)0));
							break;
						case TypeCode.Byte:
							outputList.Add((byte)obj);
							break;
						case TypeCode.Char:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((char)obj)));
							break;
						case TypeCode.DBNull:
							outputList.Add(0);
							break;
						case TypeCode.Decimal:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((double)(decimal)obj)));
							break;
						case TypeCode.Double:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((double)obj)));
							break;
						case TypeCode.Int16:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((Int16)obj)));
							break;
						case TypeCode.Int32:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((Int32)obj)));
							break;
						case TypeCode.Int64:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((Int64)obj)));
							break;
						case TypeCode.SByte:
							outputList.Add((byte)(sbyte)obj);
							break;
						case TypeCode.Single:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((Single)obj)));
							break;
						case TypeCode.UInt16:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((UInt16)obj)));
							break;
						case TypeCode.UInt32:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((UInt32)obj)));
							break;
						case TypeCode.UInt64:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((UInt64)obj)));
							break;
						case TypeCode.String:
							outputList.AddRange(GetLittleEndian(BitConverter.GetBytes((UInt16)((string)obj).Length)));
							outputList.AddRange(Encoding.Unicode.GetBytes((string)obj));
							break;
					}
				}
			}
			return outputList.ToArray();
		}

		/// <summary>
		/// Serializuje obiekty do istniejącej tablicy.
		/// 
		/// Tablica musi mieć odpowiedni rozmiar i nie może być nullem!
		/// </summary>
		/// <param name="output">Tablica.</param>
		/// <param name="objs">Obiekty.</param>
		/// <returns></returns>
		public static void StaticSerialize(byte[] output, params object[] objs)
		{
			for (int i = 0, j = 0; i < objs.Length; i++)
			{
				if (objs[i] == null)
					output[j++] = 0;
				else if (objs[i] is IConvertible)
				{
					switch ((objs[i] as IConvertible).GetTypeCode())
					{
						case TypeCode.Boolean:
							output[j++] = (bool)objs[i] ? (byte)1 : (byte)0;
							break;
						case TypeCode.Byte:
							output[j++] = (byte)objs[i];
							break;
						case TypeCode.Char:
							GetAndCopy(BitConverter.GetBytes((char)objs[i]), output, j);
							j += 2;
							break;
						case TypeCode.DBNull:
							output[j++] = 0;
							break;
						case TypeCode.Decimal:
							GetAndCopy(BitConverter.GetBytes((double)(decimal)objs[i]), output, j);
							j += 8;
							break;
						case TypeCode.Double:
							GetAndCopy(BitConverter.GetBytes((double)objs[i]), output, j);
							j += 8;
							break;
						case TypeCode.Int16:
							GetAndCopy(BitConverter.GetBytes((Int16)objs[i]), output, j);
							j += 2;
							break;
						case TypeCode.Int32:
							GetAndCopy(BitConverter.GetBytes((Int32)objs[i]), output, j);
							j += 4;
							break;
						case TypeCode.Int64:
							GetAndCopy(BitConverter.GetBytes((Int64)objs[i]), output, j);
							j += 8;
							break;
						case TypeCode.SByte:
							output[j++] = (byte)(sbyte)objs[i];
							break;
						case TypeCode.Single:
							GetAndCopy(BitConverter.GetBytes((Single)objs[i]), output, j);
							j += 4;
							break;
						case TypeCode.UInt16:
							GetAndCopy(BitConverter.GetBytes((UInt16)objs[i]), output, j);
							j += 2;
							break;
						case TypeCode.UInt32:
							GetAndCopy(BitConverter.GetBytes((UInt32)objs[i]), output, j);
							j += 4;
							break;
						case TypeCode.UInt64:
							GetAndCopy(BitConverter.GetBytes((UInt64)objs[i]), output, j);
							j += 8;
							break;
						case TypeCode.String:
							GetAndCopy(BitConverter.GetBytes((UInt16)((string)objs[i]).Length), output, j);
							j += 2;
							Array.Copy(Encoding.Unicode.GetBytes((string)objs[i]), 0, output, j, ((string)objs[i]).Length * 2);
							j += ((string)objs[i]).Length * 2;
							break;
					}
				}
			}
		}

		private static byte[] GetLittleEndian(byte[] input)
		{
			if (BitConverter.IsLittleEndian)
				return input;
			Array.Reverse(input);
			return input;
		}

		private static void GetAndCopy(byte[] input, byte[] array, int i)
		{
			Array.Copy(GetLittleEndian(input), 0, array, i, input.Length);
		}
		#endregion

		#region Private fields
		private byte[] Data = null;
		private int Index = 0;
		private byte[] TempData;
		#endregion

		#region IBinarySerializer Members
		/// <summary>
		/// Serializuje obiekty do nowej tablicy.
		/// </summary>
		/// <param name="objs">Obiekty.</param>
		/// <returns></returns>
		public byte[] Serialize(params object[] objs)
		{
			return StaticSerialize(objs);
		}

		/// <summary>
		/// Serializuje obiekty do istniejącej tablicy.
		/// 
		/// Tablica musi mieć odpowiedni rozmiar i nie może być nullem!
		/// </summary>
		/// <param name="output">Tablica.</param>
		/// <param name="objs">Obiekty.</param>
		public void Serialize(byte[] output, params object[] objs)
		{
			StaticSerialize(output, objs);
		}

		public bool GetBool()
		{
			return this.Data[this.Index++] != 0;
		}

		public byte GetByte()
		{
			return this.Data[this.Index++];
		}

		public sbyte GetSByte()
		{
			return (sbyte)this.Data[this.Index++];
		}

		public char GetChar()
		{
			if(!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				return BitConverter.ToChar(this.TempData, 0);
			}
			char tmp = BitConverter.ToChar(this.Data, this.Index);
			this.Index += 2;
			return tmp;
		}

		public short GetInt16()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				return BitConverter.ToInt16(this.TempData, 0);
			}
			Int16 tmp = BitConverter.ToInt16(this.Data, this.Index);
			this.Index += 2;
			return tmp;
		}

		public int GetInt32()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				this.TempData[2] = this.Data[this.Index++];
				this.TempData[3] = this.Data[this.Index++];
				return BitConverter.ToInt32(this.TempData, 0);
			}
			Int32 tmp = BitConverter.ToInt32(this.Data, this.Index);
			this.Index += 4;
			return tmp;
		}

		public long GetInt64()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				this.TempData[2] = this.Data[this.Index++];
				this.TempData[3] = this.Data[this.Index++];
				this.TempData[4] = this.Data[this.Index++];
				this.TempData[5] = this.Data[this.Index++];
				this.TempData[6] = this.Data[this.Index++];
				this.TempData[7] = this.Data[this.Index++];
				return BitConverter.ToInt64(this.TempData, 0);
			}
			Int64 tmp = BitConverter.ToInt64(this.Data, this.Index);
			this.Index += 8;
			return tmp;
		}

		public ushort GetUInt16()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				return BitConverter.ToUInt16(this.TempData, 0);
			}
			UInt16 tmp = BitConverter.ToUInt16(this.Data, this.Index);
			this.Index += 2;
			return tmp;
		}

		public uint GetUInt32()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				this.TempData[2] = this.Data[this.Index++];
				this.TempData[3] = this.Data[this.Index++];
				return BitConverter.ToUInt32(this.TempData, 0);
			}
			UInt32 tmp = BitConverter.ToUInt32(this.Data, this.Index);
			this.Index += 4;
			return tmp;
		}

		public ulong GetUInt64()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				this.TempData[2] = this.Data[this.Index++];
				this.TempData[3] = this.Data[this.Index++];
				this.TempData[4] = this.Data[this.Index++];
				this.TempData[5] = this.Data[this.Index++];
				this.TempData[6] = this.Data[this.Index++];
				this.TempData[7] = this.Data[this.Index++];
				return BitConverter.ToUInt64(this.TempData, 0);
			}
			UInt64 tmp = BitConverter.ToUInt64(this.Data, this.Index);
			this.Index += 8;
			return tmp;
		}

		public double GetDouble()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				this.TempData[2] = this.Data[this.Index++];
				this.TempData[3] = this.Data[this.Index++];
				this.TempData[4] = this.Data[this.Index++];
				this.TempData[5] = this.Data[this.Index++];
				this.TempData[6] = this.Data[this.Index++];
				this.TempData[7] = this.Data[this.Index++];
				return BitConverter.ToDouble(this.TempData, 0);
			}
			double tmp = BitConverter.ToDouble(this.Data, this.Index);
			this.Index += 8;
			return tmp;
		}

		public float GetFloat()
		{
			if (!BitConverter.IsLittleEndian)
			{
				this.TempData[0] = this.Data[this.Index++];
				this.TempData[1] = this.Data[this.Index++];
				this.TempData[2] = this.Data[this.Index++];
				this.TempData[3] = this.Data[this.Index++];
				return BitConverter.ToSingle(this.TempData, 0);
			}
			Single tmp = BitConverter.ToSingle(this.Data, this.Index);
			this.Index += 4;
			return tmp;
		}

		public string GetString()
		{
			ushort len = this.GetUInt16();
			string tmp = Encoding.Unicode.GetString(this.Data, this.Index, len * 2);
			this.Index += len * 2;
			return tmp;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje serializer.
		/// </summary>
		/// <param name="data">Dane do deserializacji.</param>
		/// <param name="startIndex">Początkowy indeks.</param>
		public BinarySerializer(byte[] data, int startIndex = 0)
		{
			this.Data = data;
			this.Index = startIndex;
			if (!BitConverter.IsLittleEndian)
				this.TempData = new byte[8];
		}
		#endregion
	}
}
