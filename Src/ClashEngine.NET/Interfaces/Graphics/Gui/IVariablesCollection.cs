using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kolekcja zmiennych.
	/// </summary>
	public interface IVariablesCollection
		: ICollection<IVariable>
	{
		/// <summary>
		/// Pobiera zmienną o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator zmiennej.</param>
		/// <returns></returns>
		IVariable this[string id] { get; }

		/// <summary>
		/// Sprawdza, czy w kolekcji najduje się zmienna o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		bool Contains(string id);

		/// <summary>
		/// Usuwa zmienną o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Czy udało się usunąć.</returns>
		bool Remove(string id);
	}
}
