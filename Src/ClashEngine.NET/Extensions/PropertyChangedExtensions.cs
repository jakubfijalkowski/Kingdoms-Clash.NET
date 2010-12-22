using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ClashEngine.NET.Extensions
{
	public static class PropertyChangedExtensions
	{
		/// <summary>
		/// Jeśli eventHandler nie jest pusty - wypuszcza zdarzenie.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventHandler">this</param>
		/// <param name="sender"></param>
		/// <param name="propertyName"></param>
		public static void Raise<T>(this PropertyChangedEventHandler eventHandler, T sender, string propertyName)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Jeśli eventHandler nie jest pusty - wypuszcza zdarzenie.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventHandler">this</param>
		/// <param name="sender"></param>
		/// <param name="propertyName"></param>
		public static void Raise<T>(this PropertyChangedEventHandler eventHandler, T sender, Expression<Func<T, object>> propertyExpression)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, new PropertyChangedEventArgs(sender.NameOf(propertyExpression)));
			}
		}

		/// <summary>
		/// Jeśli eventHandler nie jest pusty - wypuszcza zdarzenie.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventHandler">this</param>
		/// <param name="sender"></param>
		/// <param name="propertyName"></param>
		public static void Raise<T>(this PropertyChangedEventHandler eventHandler, T sender, Expression<Func<object>> propertyExpression)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, new PropertyChangedEventArgs(sender.NameOf(propertyExpression)));
			}
		}
	}
}
