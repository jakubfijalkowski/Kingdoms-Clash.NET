using System;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Atrybut encji gry.
	/// </summary>
	public class Attribute
		: IAttribute
	{
		/// <summary>
		/// Identyfikator atrybutu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Wartość atrybutu.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Inicjalizuje nowy atrybut.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="value">Wartość.</param>
		public Attribute(string id, object value)
		{
			this.Id = id;
			this.Value = value;
		}

		#region IEquatable<IAttribute> Members
		bool IEquatable<IAttribute>.Equals(IAttribute other)
		{
			return this.Id.Equals(other.Id);
		}
		#endregion
	}
}
