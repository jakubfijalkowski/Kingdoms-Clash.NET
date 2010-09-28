using ClashEngine.NET.EntitiesManager;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Units;

	/// <summary>
	/// Atrybut jednostki.
	/// </summary>
	public class UnitAttribute
		: Attribute, IUnitAttribute
	{
		public UnitAttribute(string id, object value)
			: base(id, value)
		{ }
	}

	/// <summary>
	/// Atrybut jednostki silnie typowany.
	/// </summary>
	public class UnitAttribute<T>
		: Attribute<T>, IUnitAttribute<T>
	{
		public UnitAttribute(string id, T value)
			: base(id, value)
		{ }
	}
}
