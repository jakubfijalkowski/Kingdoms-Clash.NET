using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Bazowy interfejs komponentu jednostki.
	/// </summary>
	/// <remarks>
	/// Wszystkie atrybuty niebędące w opisie jednostki powinny być implementowane na bazie IAttribute.
	/// </remarks>
	public interface IUnitComponent
		: IComponent
	{
		/// <summary>
		/// Opis komponentu.
		/// </summary>
		IUnitComponentDescription Description { get; set; }
	}
}
