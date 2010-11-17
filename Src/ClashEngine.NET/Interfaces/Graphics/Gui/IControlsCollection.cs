using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kolekcja kontrolek.
	/// </summary>
	public interface IControlsCollection
		: ICollection<IControl>
	{
		/// <summary>
		/// Dodaje listę kontrolek do kolekcji.
		/// </summary>
		/// <param name="items">Lista.</param>
		void AddRange(IEnumerable<IControl> items);

		/// <summary>
		/// Usuwa kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Czy udało się usunąć kontrolkę.</returns>
		bool Remove(string id);

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się kontrolka o podanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		bool Contains(string id);

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="index">Id.</param>
		/// <returns></returns>
		IControl this[string id] { get; }
	}
}
