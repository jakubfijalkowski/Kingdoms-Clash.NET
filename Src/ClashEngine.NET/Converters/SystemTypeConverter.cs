using System;
using System.ComponentModel;

namespace ClashEngine.NET.Converters
{
	/// <summary>
	/// Konwerter typów dla System.Type.
	/// </summary>
	/// <remarks>
	/// Obsługiwane konwersje na: string
	/// Obsługiwane konwersje z: string
	/// 
	/// Dodatkowo umożliwia konwersje z typów wbudowanych na typy CLR, wektorów i typu koloru wykorzystując krótszy zapis.
	/// </remarks>
	public class SystemTypeConverter
		: TypeConverter
	{
		#region From
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string)
			{
				switch ((value as string).ToLower())
				{
				case "bool":
					return typeof(bool);
				case "char":
					return typeof(char);

				case "decimal":
					return typeof(decimal);
				case "double":
					return typeof(double);
				case "float":
					return typeof(float);

				case "byte":
					return typeof(byte);
				case "sbyte":
					return typeof(sbyte);
				case "short":
					return typeof(short);
				case "ushort":
					return typeof(ushort);
				case "int":
					return typeof(int);
				case "uint":
					return typeof(uint);
				case "long":
					return typeof(long);
				case "ulong":
					return typeof(ulong);

				case "object":
					return typeof(object);
				case "string":
					return typeof(string);

				case "vector2":
					return typeof(OpenTK.Vector2);
				case "vector3":
					return typeof(OpenTK.Vector3);
				case "vector4":
					return typeof(OpenTK.Vector4);

				case "color":
					return typeof(System.Drawing.Color);
				default:
					return Type.GetType(value as string);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}
		#endregion

		#region To
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return ((Type)value).FullName;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion
	}
}
