using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Kolekcja atrybutów jednostki.
	/// Metoda przechowywania atrybutów jest dowolna.
	/// </summary>
	public interface IUnitAttributesCollection
		: ICollection<IUnitAttribute>
	{
		/// <summary>
		/// Pobiera atrybut o wskazanym identyfikatorze.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Wartość atrybutu lub null, gdy nie odnaleziono.</returns>
		object this[string id] { get; }

		/// <summary>
		/// Pobiera atrybut o wskazanym identyfikatorze.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Wartość atrybutu lub null, gdy nie odnaleziono.</returns>
		object Get(string id);

		/// <summary>
		/// Pobiera atrybut o wskazanym identyfikatorze.
		/// </summary>
		/// <typeparam name="T">Rządany typ atrybutu.</typeparam>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Wartość atrybutu lub domyślną wartość dla T, gdy nie odnaleziono.</returns>
		T Get<T>(string id);
	}
}
