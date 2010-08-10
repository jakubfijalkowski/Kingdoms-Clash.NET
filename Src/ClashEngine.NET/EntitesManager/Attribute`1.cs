using System;

namespace ClashEngine.NET.EntitiesManager
{
	/// <summary>
	/// Atrybut encji gry. Silnie typowany.
	/// </summary>
	/// <typeparam name="T">Typ atrybutu.</typeparam>
	public class Attribute<T>
		: Attribute, IEquatable<Attribute<T>>
		where T : class
	{
		/// <summary>
		/// Wartość atrybutu.
		/// </summary>
		public new T Value
		{
			get { return base.Value as T; }
			set { base.Value = value; }
		}

		/// <summary>
		/// Inicjalizuje nowy atrybut.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="value">Wartość.</param>
		public Attribute(string id, T value)
			: base(id, value)
		{ }

		#region IEquatable<Attribute<T>> Members
		bool IEquatable<Attribute<T>>.Equals(Attribute<T> other)
		{
			return this.Id.Equals(other.Id);
		}
		#endregion
	}
}
