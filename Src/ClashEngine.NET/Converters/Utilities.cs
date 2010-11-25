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
		/// <returns>Typ konwertera lub null, gdy nie znaleziono.</returns>
		public static Type GetTypeConverterFor(PropertyInfo property)
		{
			var converter = property.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			if (converter.Length == 0)
			{
				converter = property.PropertyType.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			}
			return (converter.Length == 1 ? (Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName)) : null);
		}

		/// <summary>
		/// Pobiera typ konwertera dla danego pola.
		/// </summary>
		/// <param name="field">Pole.</param>
		/// <returns>Typ konwertera lub null, gdy nie znaleziono.</returns>
		public static Type GetTypeConverterFor(FieldInfo field)
		{
			var converter = field.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			if (converter.Length == 0)
			{
				converter = field.FieldType.GetCustomAttributes(typeof(TypeConverterAttribute), false);
			}
			return (converter.Length == 1 ? (Type.GetType((converter[0] as TypeConverterAttribute).ConverterTypeName)) : null);
		}

		/// <summary>
		/// Pobiera typ konwertera.
		/// </summary>
		/// <param name="member">Właściwość lub pole.</param>
		/// <exception cref="ArgumentException">Rzucane gdy member nie jest ani PropertyInfo ani FieldInfo.</exception>
		/// <returns>Typ konwertera lub null, gdy nie znaleziono.</returns>
		public static Type GetTypeConverterFor(MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return GetTypeConverterFor(member as PropertyInfo);
			}
			else if (member is FieldInfo)
			{
				return GetTypeConverterFor(member as FieldInfo);
			}
			throw new ArgumentException("Parameter is not PropertyInfo nor FieldInfo", "mi");
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
