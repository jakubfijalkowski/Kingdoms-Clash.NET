using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Conditions
{
	/// <summary>
	/// Kolekcja wyzwalaczy.
	/// </summary>
	public interface ITriggersCollection
		: ICollection<ITrigger>
	{
		/// <summary>
		/// Wywołuje wszystkie wyzwalacze.
		/// </summary>
		void TrigAll();
	}
}
