using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Atrybut jednostki.
	/// </summary>
	public interface IUnitAttribute
		: IAttribute
	{ }

	/// <summary>
	/// Atrybut jednostki.
	/// </summary>
	/// <typeparam name="T">Jego typ.</typeparam>
	public interface IUnitAttribute<T>
		: IUnitAttribute, IAttribute<T>
	{
	}
}
