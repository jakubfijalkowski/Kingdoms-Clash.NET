using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui.Internals
{
	using Extensions;
	using Interfaces.Graphics.Gui;
	using Interfaces.Graphics.Gui.Objects;

	/// <summary>
	/// Kolekcja obiektów renderera GUI.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	internal class ObjectsCollection
		: IObjectsCollection
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IObject> Objects = new List<IObject>();
		#endregion

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
			item.Owner = this.Owner;
			item.Position = item.Position; //Wymuszamy aktualizację
			this.Objects.Add(item);
			item.OnAdd();
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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count { get { return this.Objects.Count; } }

		/// <summary>
		/// Zawsze false.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		bool ICollection<IObject>.IsReadOnly { get { return false; } }
		#endregion

		#region IList<IObject> Members
		/// <summary>
		/// Pobiera/zmienia element na wskazanej pozycji.
		/// </summary>
		/// <param name="index">Indeks.</param>
		/// <returns></returns>
		public IObject this[int index]
		{
			get { return this.Objects[index]; }
			set { this.Objects[index] = value; }
		}

		/// <summary>
		/// Wyszukuje indeksu wskazanego elementu.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(IObject item)
		{
			return this.Objects.IndexOf(item);
		}

		/// <summary>
		/// Dodaje element na wskazanej pozycji.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, IObject item)
		{
			this.Objects.Insert(index, item);
		}

		/// <summary>
		/// Usuwa element ze wskazanej pozycji.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			this.Objects.RemoveAt(index);
		}
		#endregion

		#region IObjectsCollection Members
		/// <summary>
		/// Właściciel.
		/// </summary>
		public IStylizableControl Owner { get; private set; }
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

		#region Constructors
		public ObjectsCollection(IStylizableControl owner)
		{
			this.Owner = owner;
			this.Owner.PropertyChanged += this.UpdatePositions;
		}
		#endregion

		#region Events
		/// <summary>
		/// Przy poruszeniu kontrolki-rodzica zmienia pozycję obiektów.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdatePositions(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.Owner.NameOf(_ => _.AbsolutePosition))
			{
				foreach (var item in this.Objects)
				{
					item.Position = item.Position;
				}
			}
		}
		#endregion
	}
}
