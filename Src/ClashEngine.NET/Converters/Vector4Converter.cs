using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using OpenTK;

namespace ClashEngine.NET.Converters
{
	using Extensions;

	/// <summary>
	/// Konwerter OpenTK.Vector4.
	/// </summary>
	/// <remarks>
	/// Obsługuje konwersję na: string, Color
	/// Obsługuje konwersję z: string, Color, kolor jako tekst
	/// </remarks>
	public class Vector4Converter
		: TypeConverter
	{
		#region From
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string) ||
				sourceType == typeof(Color))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				Vector4? v = TryConvertFromString(value as string);
				if (!v.HasValue)
				{
					v = TryConvertFromColorString(value as string);
				}
				if (!v.HasValue)
				{
					throw new FormatException("value is not color name nor in 'x, y, z, w' format");
				}
				return v.Value;
			}
			else if (value is Color)
			{
				return ((Color)value).ToVector4();
			}
			return base.ConvertFrom(context, culture, value);
		}

		private static Vector4? TryConvertFromString(string str)
		{
			try
			{
				string[] v = str.Split(',');
				if (v.Length == 3)
				{
					return new Vector4(
						float.Parse(v[0], CultureInfo.InvariantCulture),
						float.Parse(v[1], CultureInfo.InvariantCulture),
						float.Parse(v[2], CultureInfo.InvariantCulture),
						1
						);
				}
				else if (v.Length == 4)
				{
					return new Vector4(
						float.Parse(v[0], CultureInfo.InvariantCulture),
						float.Parse(v[1], CultureInfo.InvariantCulture),
						float.Parse(v[2], CultureInfo.InvariantCulture),
						float.Parse(v[3], CultureInfo.InvariantCulture)
						);
				}
			}
			catch
			{ }
			return null;
		}

		private static Vector4? TryConvertFromColorString(string str)
		{
			Color color = Color.FromName(str);
			if (!color.IsNamedColor)
			{
				return null;
			}
			return color.ToVector4();
		}
		#endregion

		#region To
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string) || destinationType == typeof(Color))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			Vector4 vec = (Vector4)value;
			if (destinationType == typeof(string))
			{
				return vec.X.ToString(CultureInfo.InvariantCulture) + "," + vec.Y.ToString(CultureInfo.InvariantCulture)
					+ "," + vec.Z.ToString(CultureInfo.InvariantCulture) + "," + vec.W.ToString(CultureInfo.InvariantCulture);
			}
			else if (destinationType == typeof(Color))
			{
				return vec.ToColor();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion
	}
}
