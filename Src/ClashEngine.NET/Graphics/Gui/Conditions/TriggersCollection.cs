using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kolekcja warunków do stylizacji GUI.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class TriggersCollection
		: ITriggersCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<ITrigger> Triggers = new List<ITrigger>();

		#region ICollection<ITrigger> Members
		/// <summary>
		/// Dodaje nowy element.
		/// </summary>
		/// <param name="item">Element.</param>
		public void Add(ITrigger item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Triggers.Add(item);
		}

		/// <summary>
		/// Usuwa wskazany element.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <returns>Czy udało się usunąć.</returns>
		public bool Remove(ITrigger item)
		{
			return this.Triggers.Remove(item);
		}

		/// <summary>
		/// Usuwa wszystkie.
		/// </summary>
		public void Clear()
		{
			this.Triggers.Clear();
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera podany element.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <returns></returns>
		public bool Contains(ITrigger item)
		{
			return this.Triggers.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<ITrigger>.CopyTo(ITrigger[] array, int arrayIndex)
		{
			this.Triggers.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę elementów kolekcji.
		/// </summary>
		public int Count
		{
			get { return this.Triggers.Count; }
		}

		/// <summary>
		/// Nieużywane - zawsze false.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		bool ICollection<ITrigger>.IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region ITriggersCollection Members
		/// <summary>
		/// Wywołuje wszystkie wyzwalacze.
		/// </summary>
		public void TrigAll()
		{
			foreach (var t in this.Triggers)
			{
				t.Trig();
			}
		}
		#endregion

		#region IEnumerable<ITrigger> Members
		public IEnumerator<ITrigger> GetEnumerator()
		{
			return this.Triggers.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Triggers.GetEnumerator();
		}
		#endregion
	}
}
