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
		/// Obrazek z zasobem.
		/// </summary>
		string Image { get; }
	}
}
