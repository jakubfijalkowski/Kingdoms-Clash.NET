using System;

namespace ClashEngine.NET
{
	/// <summary>
	/// Bazowa klasa dla komponentów.
	/// </summary>
	public abstract class Component
		: IEquatable<Component>
	{
		/// <summary>
		/// Identyfikator(nazwa) komponentu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Właściciel komponentu.
		/// </summary>
		public GameEntity Owner { get; internal set; }

		/// <summary>
		/// Inicjalizuje nowy komponent.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public Component(string id)
		{
			this.Id = id;
		}

		/// <summary>
		/// Wywoływane przy uaktualnieniu.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		public abstract void Update(double delta);

		#region IEquatable<Component> members
		bool IEquatable<Component>.Equals(Component other)
		{
			return this.Id.Equals(other.Id);
		}
		#endregion
	}
}
