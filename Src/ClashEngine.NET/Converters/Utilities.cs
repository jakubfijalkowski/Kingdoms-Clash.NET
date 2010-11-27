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
		public static Type GetTypeConverter(PropertyInfo property, Type @default = null)
		{
			var converter = property.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			if (converter.Length == 0)
			{
				converter = property.PropertyType.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			}
			return (converter.Length == 1 ? (Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName)) : @default);
		}

		/// <summary>
		/// Pobiera typ konwertera dla danego pola.
		/// </summary>
		/// <param name="field">Pole.</param>
		/// <param name="default">Domyślny konwerter typów(jeśli nie znaleziono innego).</param>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static Type GetTypeConverter(FieldInfo field, Type @default = null)
		{
			var converter = field.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			if (converter.Length == 0)
			{
				converter = field.FieldType.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			}
			return (converter.Length == 1 ? (Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName)) : @default);
		}

		/// <summary>
		/// Pobiera typ konwertera.
		/// </summary>
		/// <param name="member">Właściwość lub pole.</param>
		/// <param name="default">Domyślny konwerter typów(jeśli nie znaleziono innego).</param>
		/// <exception cref="ArgumentException">Rzucane gdy member nie jest ani PropertyInfo ani FieldInfo.</exception>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static Type GetTypeConverter(MemberInfo member, Type @default = null)
		{
			if (member is PropertyInfo)
			{
				return GetTypeConverter(member as PropertyInfo, @default);
			}
			else if (member is FieldInfo)
			{
				return GetTypeConverter(member as FieldInfo, @default);
			}
			throw new ArgumentException("Parameter is not PropertyInfo nor FieldInfo", "mi");
		}

		/// <summary>
		/// Pobiera konwerter dla danego typu.
		/// </summary>
		/// <param name="type">Typ.</param>
		/// <param name="default">Domyślny konwerter typów(jeśli nie znaleziono innego).</param>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static Type GetTypeConverter(Type type, Type @default = null)
		{
			var converter = type.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			return (converter.Length == 1 ? (Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName)) : @default);
		}

		/// <summary>
		/// Pobiera konwerter dla danego typu.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <param name="default">Domyślny konwerter typów(jeśli nie znaleziono innego).</param>
		/// <returns>Typ konwertera lub default, gdy nie znaleziono.</returns>
		public static Type GetTypeConverter<T>(Type @default = null)
		{
			return GetTypeConverter(typeof(T));
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
