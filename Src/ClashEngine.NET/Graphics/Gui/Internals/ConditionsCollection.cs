using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui.Internals
{
	using Interfaces.Graphics.Gui.Conditions;

	/// <summary>
	/// Kolekcja warunków do stylizacji GUI.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	internal class ConditionsCollection
		: IConditionsCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<ICondition> Conditions = new List<ICondition>();

		#region ICollection<ICondition> Members
		/// <summary>
		/// Dodaje nowy element.
		/// </summary>
		/// <param name="item">Element.</param>
		public void Add(ICondition item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Conditions.Add(item);
		}

		/// <summary>
		/// Usuwa wskazany element.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <returns>Czy udało się usunąć.</returns>
		public bool Remove(ICondition item)
		{
			return this.Conditions.Remove(item);
		}

		/// <summary>
		/// Usuwa wszystkie.
		/// </summary>
		public void Clear()
		{
			this.Conditions.Clear();
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera podany element.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <returns></returns>
		public bool Contains(ICondition item)
		{
			return this.Conditions.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<ICondition>.CopyTo(ICondition[] array, int arrayIndex)
		{
			this.Conditions.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę elementów kolekcji.
		/// </summary>
		public int Count
		{
			get { return this.Conditions.Count; }
		}

		/// <summary>
		/// Nieużywane - zawsze false.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		bool ICollection<ICondition>.IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<ICondition> Members
		public IEnumerator<ICondition> GetEnumerator()
		{
			return this.Conditions.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Conditions.GetEnumerator();
		}
		#endregion
	}
}
