using OpenTK;

namespace Kingdoms_Clash.NET.Interfaces.Resources
{
	/// <summary>
	/// Opis zasobu.
	/// Używane tylko do opisywania zasobów np. w GUI, nie używane w grze.
	/// </summary>
	public interface IResourceDescription
	{
		/// <summary>
		/// Identyfikator(game-friendly) zasobu.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Nazwa(user-friendly) zasobu.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Dłuższy opis.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Rozmiar zasobu w grze.
		/// </summary>
		OpenTK.Vector2 Size { get; }

		/// <summary>
		/// Tekstura dla zasobu.
		/// </summary>
		string Image { get; }

		/// <summary>
		/// Figura, która jest przybliżeniem zasobu. Używane do prezentacji fizyki.
		/// </summary>
		Vector2[] Polygon { get; }
	}
}
