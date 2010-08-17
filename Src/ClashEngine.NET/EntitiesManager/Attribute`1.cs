using System;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Atrybut encji gry. Silnie typowany.
	/// </summary>
	/// <typeparam name="T">Typ atrybutu.</typeparam>
	public class Attribute<T>
		: Attribute, IAttribute<T>
	{
		/// <summary>
		/// Wartość atrybutu.
		/// </summary>
		public new T Value
		{
			get { return (T)base.Value; }
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
		bool IEquatable<IAttribute<T>>.Equals(IAttribute<T> other)
		{
			return this.Id.Equals(other.Id);
		}
		#endregion
	}
}
