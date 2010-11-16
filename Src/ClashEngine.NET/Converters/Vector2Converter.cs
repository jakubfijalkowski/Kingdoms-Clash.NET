using System;
using System.ComponentModel;
using System.Globalization;
using OpenTK;

namespace ClashEngine.NET.Converters
{
	/// <summary>
	/// Konwerter OpenTK.Vector2.
	/// </summary>
	/// <remarks>
	/// Obsługiwane konwersje na: string
	/// Obsługiwane konwersje z: string
	/// </remarks>
	public class Vector2Converter
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

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string[] v = (value as string).Split(',');
				return new Vector2(
					float.Parse(v[0].Trim(), CultureInfo.InvariantCulture),
					float.Parse(v[1].Trim(), CultureInfo.InvariantCulture)
					);
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

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				Vector2 vec = (Vector2)value;
				return vec.X.ToString(CultureInfo.InvariantCulture) + "," + vec.Y.ToString(CultureInfo.InvariantCulture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion
	}
}
