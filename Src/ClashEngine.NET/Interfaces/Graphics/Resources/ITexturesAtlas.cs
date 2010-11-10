using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Resources
{
	/// <summary>
	/// Interfejs dla atlasu tekstur.
	/// </summary>
	public interface ITexturesAtlas
		: IResource, IEnumerable<ITexture>
	{
		/// <summary>
		/// Kolekcja z identyfikatorami tekstur, które znajdują się w atlasie.
		/// Tylko do odczytu.
		/// </summary>
		ICollection<string> IDs { get; }

		/// <summary>
		/// Pobiera teksturę o wskazanum ID.
		/// </summary>
		/// <param name="id">Identyfikator tekstury.</param>
		/// <returns>Tekstura.</returns>
		ITexture this[string id] { get; }
	}
}
