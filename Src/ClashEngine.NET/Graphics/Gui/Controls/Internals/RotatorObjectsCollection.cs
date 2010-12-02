using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClashEngine.NET.Graphics.Gui.Controls.Internals
{
	using Interfaces.Graphics.Gui.Controls;

	/// <summary>
	/// Kolekcja obiektów rotatora.
	/// Wymaga, by wszystkie obiekty w kolekcji były tego samego typu.
	/// </summary>
	internal class RotatorObjectsCollection
		: IRotatorObjectsCollection
	{
		#region Private fields
		private List<object> Objects = new List<object>();
		private Rotator Rotator = null;
		#endregion

		#region IRotatorObjectsCollection Members
		/// <summary>
		/// Typ obiektów w kolekcji.
		/// </summary>
		public Type ObjectsType { get; private set; }

		/// <summary>
		/// Pobiera obiekt na wskazanej pozycji.
		/// </summary>
		/// <param name="index">Indeks.</param>
		/// <returns>Element lub null, gdy index jest poza zakresem.</returns>
		public object this[int index]
		{
			get { return (index < this.Objects.Count ? this.Objects[index] : null); }
		}
		#endregion

		#region ICollection<object> Members
		/// <summary>
		/// Dodaje element do kolekcji.
		/// </summary>
		/// <param name="item"></param>
		public void Add(object item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.ObjectsType == null)
			{
				this.ObjectsType = item.GetType();
			}
			else if (!this.ObjectsType.IsInstanceOfType(item))
			{
				throw new ArgumentException(string.Format("Item is not of type {0}", this.ObjectsType.Name), "item");
			}
			this.Objects.Add(item);
			this.Rotator.SendItemChanged(this.Count - 1);
		}

		/// <summary>
		/// Usuwa wskazany element.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(object item)
		{
			if (this.Objects.Remove(item))
			{
				this.Rotator.SendItemChanged(this.Count);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Objects.Clear();
			this.Rotator.SendItemChanged(-1);
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wskazany element.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(object item)
		{
			return this.Objects.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<object>.CopyTo(object[] array, int arrayIndex)
		{
			this.Objects.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę elementów.
		/// </summary>
		public int Count
		{
			get { return this.Objects.Count; }
		}

		/// <summary>
		/// Nieużywane - zawsze false.
		/// </summary>
		bool ICollection<object>.IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<object> Members
		public IEnumerator<object> GetEnumerator()
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

		#region Constructors
		public RotatorObjectsCollection(Rotator rotator)
		{
			this.Rotator = rotator;
		}		
		#endregion
	}
}
