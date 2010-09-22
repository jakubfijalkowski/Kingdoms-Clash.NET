using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.PhysicsManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Komponent fizyczny.
	/// Po dodaniu do encji staje się ona zależna od wszystkich czynników zewnętrznych.
	/// 
	/// Wszystkie właściwości powinny być zaimplementowane na atrybutach.
	/// </summary>
	public interface IPhysics
		: IComponent
	{
		/// <summary>
		/// Lista tylko do odczytu z prędkościami od których jest zależny dany obiekt.
		/// </summary>
		IList<IVelocity> Velocities { get; }

		/// <summary>
		/// Dodaje kolejną prędkość do komponentu.
		/// </summary>
		/// <param name="velocity">Prędkość.</param>
		void Add(IVelocity velocity);
		
		/// <summary>
		/// Usuwa wskazaną prędkość.
		/// </summary>
		/// <param name="velocity">Prędkość do usunięcia.</param>
		void Remove(IVelocity velocity);
	}
}
