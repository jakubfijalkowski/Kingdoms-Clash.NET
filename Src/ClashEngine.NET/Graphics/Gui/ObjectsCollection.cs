using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kolekcja obiektów renderera GUI.
	/// </summary>
	public class ObjectsCollection
		: IObjectsCollection
	{
		private List<IObject> Objects = new List<IObject>();

		#region ICollection<IObject> Members
		/// <summary>
		/// Dodaje obiekt do kolekcji.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IObject item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.Objects.Add(item);
		}

		/// <summary>
		/// Usuwa wskazany obiekt.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(IObject item)
		{
			return this.Objects.Remove(item);
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Objects.Clear();
		}

		/// <summary>
		/// Sprawdza, czy podany obiekt znajduje się w kolekcji.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(IObject item)
		{
			return this.Objects.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcję do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<IObject>.CopyTo(IObject[] array, int arrayIndex)
		{
			this.Objects.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba elementów.
		/// </summary>
		public int Count { get { return this.Objects.Count; } }

		/// <summary>
		/// Zawsze false.
		/// </summary>
		bool ICollection<IObject>.IsReadOnly { get { return false; } }
		#endregion

		#region IEnumerable<IObject> Members
		public IEnumerator<IObject> GetEnumerator()
		{
			return this.Objects.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Objects.GetEnumerator();
		}
		#endregion
	}
}
