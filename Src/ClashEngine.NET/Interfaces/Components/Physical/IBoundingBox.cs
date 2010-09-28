using FarseerPhysics.Dynamics;

namespace ClashEngine.NET.Interfaces.Components.Physical
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Dodaje wielokąt do ciała obiektu okalający go.
	/// </summary>
	public interface IBoundingBox
		: IComponent
	{
		/// <summary>
		/// Fixture.
		/// </summary>
		Fixture Fixture { get; }
	}
}
