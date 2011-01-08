using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using ClashEngine.NET.Interfaces.Graphics.Gui.Layout;

namespace ClashEngine.NET.Converters
{
	/// <summary>
	/// Konwerter typów dla Graphics.Gui.Layout.ILayoutEngine.
	/// </summary>
	/// <remarks>
	/// Obsługiwane konwersje na: string
	/// Obsługiwane konwersje z: string
	/// </remarks>
	public class LayoutConverter
		: TypeConverter
	{
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
			if (destinationType == typeof(string) && value != null &&
				value.GetType().Assembly == Assembly.GetExecutingAssembly())
			{
				return (value.GetType().Name.EndsWith("Layout") ? value.GetType().Name.Substring(0, value.GetType().Name.Length - 6) : value.GetType().Name);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		#endregion

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
				string name = (value as string).ToLower();
				var types = Assembly.GetExecutingAssembly().GetExportedTypes();
				var type = types.FirstOrDefault(_ => (_.Name.ToLower() == name || _.Name.ToLower() == name + "layout")
					&& _.GetInterfaces().Any(i => i == typeof(ILayoutEngine))
					&& _.GetConstructor(Type.EmptyTypes) != null);
				if (type != null)
				{
					return Activator.CreateInstance(type);
				}
				return null;
			}
			return base.ConvertFrom(context, culture, value);
		}
		#endregion
	}
}
