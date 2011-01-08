using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.Graphics.Gui
{
	/// <summary>
	/// Kolekcja kontrolek.
	/// Ma za zadanie ustawić właściwości Data, Owner i ContainerOffset kontrolek do niej dodanych.
	/// </summary>
	/// <seealso cref="IContainer"/>
	public interface IControlsCollection
		: ICollection<IControl>, IList<IControl>
	{
		/// <summary>
		/// Właściciel.
		/// </summary>
		IContainerControl Owner { get; }

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="index">Id.</param>
		/// <returns></returns>
		IControl this[string id] { get; }

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// Może to być kontrolka-dziecko, albo kontrolka zawarta w kontrolce-dziecku.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Kontrolka.</returns>
		IControl Get(string id);

		/// <summary>
		/// Dodaje listę kontrolek do kolekcji.
		/// </summary>
		/// <param name="items">Lista.</param>
		void AddRange(IEnumerable<IControl> items);

		/// <summary>
		/// Dodaje kontrolkę, która jest w kontrolce niżej.
		/// </summary>
		/// <remarks>
		/// Takie dodanie kontrolki nie zmienia jej właściwości a jedynie umożliwia sprawdzanie stanu kontrolki z poziomu korzenia.
		/// Nie powinny być w głównej liście, tylko w wewnętrznej, tak, by layouter nie miał do nich dostępu.
		/// Takie kontrolki muszą być dostępne z poziomu <see cref="Get"/> i <see cref="RemoveChild"/>.
		/// </remarks>
		/// <param name="control"></param>
		void AddChild(IControl control);

		/// <summary>
		/// Usuwa kontrolkę, która jest w kontrolce niżej.
		/// </summary>
		/// <param name="control"></param>
		bool RemoveChild(IControl control);

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
	}
}
