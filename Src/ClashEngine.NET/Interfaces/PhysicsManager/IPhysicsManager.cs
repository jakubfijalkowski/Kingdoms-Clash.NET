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
		IVelocitiesCollection Velocities { get; }

		/// <summary>
		/// Sumuje wektory prędkości.
		/// Pozwala na wyłączenie wskazanych prędkości.
		/// </summary>
		Vector2 CalculateVelocities(params string[] exclude);
	}
}
