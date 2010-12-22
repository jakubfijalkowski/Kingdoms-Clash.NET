using System;
using System.Linq.Expressions;

namespace ClashEngine.NET.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Pobiera nazwę właściwości z wyrażenia lambda.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		/// <param name="propertyExpression"></param>
		/// <returns></returns>
		public static string NameOf<T>(this T target, Expression<Func<T, object>> propertyExpression)
		{
			//http://blog.decarufel.net/2010/03/how-to-use-strongly-typed-name-with.html
			MemberExpression body = null;
			if (propertyExpression.Body is UnaryExpression)
			{
				var unary = propertyExpression.Body as UnaryExpression;
				if (unary.Operand is MemberExpression)
					body = unary.Operand as MemberExpression;
			}
			else if (propertyExpression.Body is MemberExpression)
			{
				body = propertyExpression.Body as MemberExpression;
			}
			if (body == null)
				throw new ArgumentException("Should be a member expression", "propertyExpression");

			// Extract the right part (after "=>")
			var vmExpression = body.Expression as ConstantExpression;

			// Extract the name of the property to raise a change on
			return body.Member.Name;
		}

		/// <summary>
		/// Pobiera nazwę właściwości z wyrażenia lambda.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		/// <param name="propertyExpression"></param>
		/// <returns></returns>
		public static string NameOf<T>(this T target, Expression<Func<object>> propertyExpression)
		{
			//http://blog.decarufel.net/2010/03/how-to-use-strongly-typed-name-with.html
			MemberExpression body = null;
			if (propertyExpression.Body is UnaryExpression)
			{
				var unary = propertyExpression.Body as UnaryExpression;
				if (unary.Operand is MemberExpression)
					body = unary.Operand as MemberExpression;
			}
			else if (propertyExpression.Body is MemberExpression)
			{
				body = propertyExpression.Body as MemberExpression;
			}
			if (body == null)
				throw new ArgumentException("Should be a member expression", "propertyExpression");

			// Extract the right part (after "=>")
			var vmExpression = body.Expression as ConstantExpression;

			// Extract the name of the property to raise a change on
			return body.Member.Name;
		}
	}
}
