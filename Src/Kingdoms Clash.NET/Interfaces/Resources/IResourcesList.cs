using System.Collections;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Resources
{
	/// <summary>
	/// Interfejs dla listy zasobów obsługiwanych przez grę.
	/// Lista zasobów jest niezmienna dla danej wersji gry.
	/// </summary>
	public interface IResourcesList
		: IEnumerable<IResourceDescription>, IEnumerable
	{
		/// <summary>
		/// Pobiera opis zasobu dla wskazanego Id.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <returns>Opis bądź null, gdy taki zasób nie istnieje.</returns>
		IResourceDescription this[string id] { get; }
	}
}
