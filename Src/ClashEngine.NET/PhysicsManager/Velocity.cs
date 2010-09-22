using System;
using OpenTK;

namespace ClashEngine.NET.PhysicsManager
{
	using Interfaces.PhysicsManager;

	/// <summary>
	/// Prędkość.
	/// </summary>
	public class Velocity
		: IVelocity
	{
		#region IVelocity Members
		/// <summary>
		/// Nazwa(unikatowa dla danej prędkości).
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Wartość.
		/// </summary>
		public Vector2 Value { get; private set; }
		#endregion

		/// <summary>
		/// Inicjalizuje nową prędkość.
		/// </summary>
		/// <param name="name">Nazwa.</param>
		/// <param name="value">Wartość.</param>
		public Velocity(string name, Vector2 value)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this.Value = value;
		}

		#region IEquatable<IVelocity> Members
		public bool Equals(IVelocity other)
		{
			return this.Name == other.Name;
		}
		#endregion
	}
}
