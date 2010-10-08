using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Interfaces.Resources
{
	/// <summary>
	/// Kolekcja zasobów.
	/// Nie powinna blokować dodawania zasobu o takim samym Id a jedynie dodawać wartość.
	/// 
	/// Klucz - identyfikator.
	/// Wartość - wartość zasobu.
	/// </summary>
	/// <seealso cref="Kingdoms_Clash.NET.Resources.ResourcesCollection"/>
	public interface IResourcesCollection
		: IDictionary<string, uint>
	{ }
}
