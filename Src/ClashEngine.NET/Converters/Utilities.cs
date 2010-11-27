using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;

namespace ClashEngine.NET.Converters
{
	/// <summary>
	/// Dodatkowe metody, które ułatwiają pobieranie konwerterów.
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Pobiera typ konwertera dla danej właściwości.
		/// </summary>
		/// <param name="property">Właściwość.</param>
		/// <param name="default">Domyślny konwerter typów(jeśli nie znaleziono innego).</param>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static TypeConverter GetTypeConverter(PropertyInfo property, Type @default = null)
		{
			var converter = property.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			return (converter.Length == 1 ?
				Activator.CreateInstance((Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName))) as TypeConverter :
				TypeDescriptor.GetConverter(property.PropertyType));
		}

		/// <summary>
		/// Pobiera typ konwertera dla danego pola.
		/// </summary>
		/// <param name="field">Pole.</param>
		/// <param name="default">Domyślny konwerter typów(jeśli nie znaleziono innego).</param>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static TypeConverter GetTypeConverter(FieldInfo field)
		{
			var converter = field.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			return (converter.Length == 1 ?
				Activator.CreateInstance((Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName))) as TypeConverter :
				TypeDescriptor.GetConverter(field.FieldType));
		}

		/// <summary>
		/// Pobiera typ konwertera.
		/// </summary>
		/// <param name="member">Właściwość lub pole.</param>
		/// <exception cref="ArgumentException">Rzucane gdy member nie jest ani PropertyInfo ani FieldInfo.</exception>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static TypeConverter GetTypeConverter(MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return GetTypeConverter(member as PropertyInfo);
			}
			else if (member is FieldInfo)
			{
				return GetTypeConverter(member as FieldInfo);
			}
			throw new ArgumentException("Parameter is not PropertyInfo nor FieldInfo", "member");
		}

		/// <summary>
		/// Pobiera konwerter dla danego typu.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static TypeConverter GetTypeConverter<T>()
		{
			return TypeDescriptor.GetConverter(typeof(T));
		}

		#region Internals
		/// <summary>
		/// Pobiera jakiś zasób z kontekstu.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		internal static Interfaces.IResource GetResource(ITypeDescriptorContext context)
		{
			var root = context.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (root != null && root.RootObject is Interfaces.IResource)
			{
				return (root.RootObject as Interfaces.IResource);
			}
			else
			{
				var target = context.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
				if (target != null && target.TargetObject is Interfaces.IResource)
				{
					return target.TargetObject as Interfaces.IResource;
				}
			}
			return null;
		}
		#endregion
	}
}
