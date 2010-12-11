using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui.Internals
{
	using Interfaces.Graphics.Gui;

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
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IControl Owner = null;
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
			item.ParentControl = this.Owner;
			item.Position = item.Position; //Wymuszamy aktualizację
			this.Objects.Add(item);
			item.Finish();
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

		#region Internal methods
		internal void UpdatePositions()
		{
			foreach (var item in this.Objects)
			{
				item.Position = item.Position;
			}
		}
		#endregion

		#region Constructors
		public ObjectsCollection(IControl owner)
		{
			this.Owner = owner;
		}
		#endregion
	}
}
