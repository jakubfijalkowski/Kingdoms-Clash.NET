using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Net.Internals
{
	using Interfaces.Net;

	/// <summary>
	/// Wewnętrzna kolekcja klientów.
	/// </summary>
	internal class ServerClientsCollection
		: IClientsCollection
	{
		#region Private fields
		private List<IClient> Clients = new List<IClient>();
		#endregion

		#region IList<IClient> Members
		/// <summary>
		/// Pobiera indeks wskazanego elementu.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(IClient item)
		{
			return this.Clients.IndexOf(item);
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, IClient item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Pobiera element ze wskazanego miejsca.
		/// Zmiana elementu jest niewspierana.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public IClient this[int index]
		{
			get { return this.Clients[index]; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		#region ICollection<IClient> Members
		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IClient item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(IClient item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		public void Clear()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera podaney element.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(IClient item)
		{
			return this.Clients.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcję do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(IClient[] array, int arrayIndex)
		{
			this.Clients.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę elementów w kolekcji.
		/// </summary>
		public int Count
		{
			get { return this.Clients.Count; }
		}

		/// <summary>
		/// Klasy nieupoważnione nie mogą zmieniać tej kolekcji.
		/// </summary>
		public bool IsReadOnly
		{
			get { return true; }
		}
		#endregion

		#region IEnumerable<IClient> Members
		public IEnumerator<IClient> GetEnumerator()
		{
			return this.Clients.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Clients.GetEnumerator();
		}
		#endregion

		#region Internals
		internal void InternalAdd(IClient item)
		{
			this.Clients.Add(item);
		}

		internal void InternalRemoveAt(int index)
		{
			this.Clients.RemoveAt(index);
		}

		internal void InternalClear()
		{
			this.Clients.Clear();
		}
		#endregion
	}
}
