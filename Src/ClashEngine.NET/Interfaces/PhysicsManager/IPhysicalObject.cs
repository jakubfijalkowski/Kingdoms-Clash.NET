using OpenTK;

namespace ClashEngine.NET.Interfaces.PhysicsManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Komponent fizyczny.
	/// Po dodaniu do encji staje się ona zależna od wszystkich czynników zewnętrznych.
	/// 
	/// Wszystkie właściwości powinny być zaimplementowane na atrybutach.
	/// </summary>
	public interface IPhysicalObject
		: IComponent
	{
		/// <summary>
		/// Lista z prędkościami od których jest zależny dany obiekt.
		/// </summary>
		IVelocitiesCollection Velocities { get; }

		/// <summary>
		/// Pozycja obiektu.
		/// </summary>
		Vector2 Position { get; set; }
	}
}
