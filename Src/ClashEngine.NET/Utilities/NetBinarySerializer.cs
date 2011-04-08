using System;
using System.Collections.Generic;
using System.Text;

namespace ClashEngine.NET.Utilities
{
	/// <summary>
	/// Helper przy serializowaniu danych binarnych.
	/// </summary>
	public static class NetBinarySerializer
	{
		public static byte[] Serialize(params object[] objs)
		{
			List<byte> outputList = new List<byte>();
			foreach (var obj in objs)
			{
				if (obj == null)
					outputList.Add(0);
				else if(obj is IConvertible)
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

		public static void Serialize(byte[] output, params object[] objs)
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
							j += ((string)objs[i]).Length;
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
	}
}
