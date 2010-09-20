using System;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Units
{
	/// <summary>
	/// Bazowy interfejs komponentu jednostki.
	/// 
	/// Dziedziczymy z ICloneable ponieważ ten interfejs jest używany zarówno w samej jednostce jak i jej opisie, więc musimy
	/// mieć możliwość tworzenia wielu instancji komponentu dla każdej jednostki.
	/// </summary>
	/// <remarks>
	/// Klasy/interfejsy dziedziczące powinny implementować atrybuty jako IAttribute,
	/// udostępnianie ich jako właściwość klasy może odbywać się już jako normalny, nie IAttribute, typ.
	/// </remarks>
	public interface IUnitComponent
		: IComponent, ICloneable
	{ }
}
