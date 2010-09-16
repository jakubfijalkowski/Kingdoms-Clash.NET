using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Bazowy interfejs komponentu jednostki.
	/// </summary>
	/// <remarks>
	/// Klasy/interfejsy dziedziczące powinny implementować atrybuty jako IAttribute,
	/// udostępnianie ich jako właściwość klasy może odbywać się już jako normalny, nie IAttribute, typ.
	/// </remarks>
	public interface IUnitComponent
		: IComponent
	{ }
}
