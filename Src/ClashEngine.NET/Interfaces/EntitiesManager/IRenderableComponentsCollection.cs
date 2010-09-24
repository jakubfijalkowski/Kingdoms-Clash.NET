using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Kolekcja komponentów które potrafią się renderować.
	/// Zawsze tylko do odczytu.
	/// </summary>
	public interface IRenderableComponentsCollection
		: ICollection<IRenderableComponent>
	{ }
}
