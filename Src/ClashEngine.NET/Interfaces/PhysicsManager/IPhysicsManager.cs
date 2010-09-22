using System.Collections.Generic;
using OpenTK;

namespace ClashEngine.NET.Interfaces.PhysicsManager
{
	/// <summary>
	/// Manager fizyki.
	/// Określa globalne parametry dla obiektów.
	/// 
	/// Powinien być jeden na całą aplikację.
	/// </summary>
	public interface IPhysicsManager
	{
		/// <summary>
		/// Prędkości.
		/// </summary>
		IList<IVelocity> Velocities { get; }

		/// <summary>
		/// Suma wektorów prędkości.
		/// </summary>
		Vector2 CalculatedVelocity { get; }

		/// <summary>
		/// Dodaje prędkość do listy.
		/// </summary>
		/// <param name="velocity">Prędkość.</param>
		void Add(IVelocity velocity);

		/// <summary>
		/// Usuwa prędkość z listy.
		/// </summary>
		/// <param name="velocity">Prędkość.</param>
		void Remove(IVelocity velocity);
	}
}
