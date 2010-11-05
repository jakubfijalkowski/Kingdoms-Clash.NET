using FarseerPhysics.Dynamics;

namespace ClashEngine.NET.Interfaces.Components
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Komponent zmieniający encję w encję fizyczną(jest uzależniona od fizyki i managera fizyki).
	/// </summary>
	/// <seealso cref="IPhysicsManager"/>
	public interface IPhysicalObject
		: IComponent
	{
		/// <summary>
		/// Ciało obiektu.
		/// Powinno być zaimplementowane jako atrybut.
		/// </summary>
		Body Body { get; }
	}
}
