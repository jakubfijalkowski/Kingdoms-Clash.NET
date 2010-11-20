using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kolekcja zmiennych XAML.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class VariablesCollection
		: IVariablesCollection
	{
		#region Private fields
		/// <summary>
		/// Lista zmiennych.
		/// </summary>
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
		private List<IVariable> Variables = new List<IVariable>();
		#endregion

		#region IVariablesCollection Members
		/// <summary>
		/// Pobiera zmienną o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator zmiennej.</param>
		/// <returns></returns>
		public IVariable this[string id]
		{
			get { return this.Variables.Find(v => v.Id == id); }
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji najduje się zmienna o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public bool Contains(string id)
		{
			return this.Variables.Find(v => v.Id == id) != null;
		}

		/// <summary>
		/// Usuwa zmienną o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Czy udało się usunąć.</returns>
		public bool Remove(string id)
		{
			return this.Variables.RemoveAll(v => v.Id == id) == 1;
		}
		#endregion

		#region ICollection<IVariable> Members
		/// <summary>
		/// Dodaje do kolekcji wskazaną zmienną.
		/// </summary>
		/// <param name="item">Zmienna.</param>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">W kolekcji istnieje element o Id równym wskazanemu.</exception>
		public void Add(IVariable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.Contains(item.Id))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Variables.Add(item);
		}

		/// <summary>
		/// Usuwa element o Id równym wksazanemu.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		bool ICollection<IVariable>.Remove(IVariable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Remove(item.Id);
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Variables.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się element o Id równym wskazanemu.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		bool ICollection<IVariable>.Contains(IVariable item)
		{
			if (item == null)
			{
				throw new ArgumentNullException();
			}
			return this.Contains(item.Id);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy/
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<IVariable>.CopyTo(IVariable[] array, int arrayIndex)
		{
			this.Variables.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba elementów.
		/// </summary>
		public int Count
		{
			get { return this.Variables.Count; }
		}

		/// <summary>
		/// Czy jest tylko do odczytu - zawsze false.
		/// </summary>
		bool ICollection<IVariable>.IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IVariable> Members
		public IEnumerator<IVariable> GetEnumerator()
		{
			return this.Variables.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Variables.GetEnumerator();
		}
		#endregion
	}
}
