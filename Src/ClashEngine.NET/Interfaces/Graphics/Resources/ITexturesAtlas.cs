using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Resources
{
	/// <summary>
	/// Interfejs dla atlasu tekstur.
	/// </summary>
	public interface ITexturesAtlas
		: ResourcesManager.IResource
	{
		/// <summary>
		/// Pobiera wszystkie identyfikatory tekstur jakie są w atlasie.
		/// </summary>
		ICollection<string> IDs { get; }

		/// <summary>
		/// Pobiera teksturę o wskazanum ID.
		/// </summary>
		/// <param name="id">Identyfikator tekstury.</param>
		/// <returns>Tekstura.</returns>
		ITexture Get(string id);
	}
}
