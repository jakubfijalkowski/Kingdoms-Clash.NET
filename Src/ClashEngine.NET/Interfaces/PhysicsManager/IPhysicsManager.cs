using OpenTK;

namespace ClashEngine.NET.Interfaces.PhysicsManager
{
	using Interfaces.Components;

	/// <summary>
	/// Manager fizyki.
	/// Określa globalne parametry dla obiektów.
	/// 
	/// Powinien być jeden na całą aplikację.
	/// </summary>
	public interface IPhysicsManager
	{
		/// <summary>
		/// Teren.
		/// </summary>
		ITerrain Terrain { get; set; }

		/// <summary>
		/// Prędkości.
		/// </summary>
		IVelocitiesCollection Velocities { get; }

		/// <summary>
		/// Sumuje wektory prędkości.
		/// Pozwala na wyłączenie wskazanych prędkości.
		/// </summary>
		Vector2 CalculateVelocities(params string[] exclude);
	}
}
