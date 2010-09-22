using System;
using OpenTK;

namespace ClashEngine.NET.Interfaces.PhysicsManager
{
	/// <summary>
	/// Określa prędkość.
	/// </summary>
	public interface IVelocity
		: IEquatable<IVelocity>
	{
		/// <summary>
		/// Nazwa prędkości(unikatowa dla danej prędkości).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Wartość.
		/// </summary>
		Vector2 Value { get; }
	}
}
