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
		/// Pozycja prostokąta w ciele.
		/// </summary>
		Vector2 Position { get; }

		/// <summary>
		/// Fixture.
		/// </summary>
		Fixture Fixture { get; }
	}
}
