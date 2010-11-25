using System;
using System.ComponentModel;

namespace ClashEngine.NET.Converters
{
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Konwertuje Graphics.Resources.Texture.
	/// </summary>
	/// <remarks>
	/// Obsługiwane konwersje na: string
	/// Obsługiwane konwersje z: string
	/// </remarks>
	public class TextureConverter
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
				var res = Utilities.GetResource(context);
				if (res != null)
				{
					return res.Manager.Load<Graphics.Resources.Texture>(value as string);
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
			if (value is ITexture && destinationType == typeof(string))
			{
				return (value as ITexture).Id;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion
	}
}
