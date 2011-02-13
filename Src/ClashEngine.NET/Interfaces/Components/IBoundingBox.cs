using FarseerPhysics.Dynamics;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Components
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Dodaje wielokąt do ciała obiektu okalający go.
	/// </summary>
	public interface IBoundingBox
		: IComponent
	{
		/// <summary>
		/// Rozmiar prostokąta.
		/// </summary>
		Vector2 Size { get; }

		/// <summary>
		/// Masa prostokąta.
		/// </summary>
		float Mass { get; }

		/// <summary>
		/// Fixture.
		/// </summary>
		Fixture Fixture { get; }
	}
}
