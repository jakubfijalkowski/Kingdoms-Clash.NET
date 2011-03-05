using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Net.Internals
{
	using Interfaces.Net;

	/// <summary>
	/// Kolekcja wiadomości.
	/// </summary>
	internal class MessagesCollection
		: IMessagesCollection
	{
		#region Private fields
		private List<Message> Messages = new List<Message>();
		#endregion

		#region IList<Message> Members
		/// <summary>
		/// Pobiera indeks wskazanego elementu.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(Message item)
		{
			return this.Messages.IndexOf(item);
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, Message item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Usuwa element ze wskazanego miejsca.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			this.Messages.RemoveAt(index);
		}

		/// <summary>
		/// Pobiera element ze wskazanego miejsca.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Message this[int index]
		{
			get { return this.Messages[index]; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		#region ICollection<Message> Members
		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		public void Add(Message item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Usuwa element z kolekcji.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(Message item)
		{
			return this.Messages.Remove(item);
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Messages.Clear();
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera wskazany element.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(Message item)
		{
			return this.Messages.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcję do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(Message[] array, int arrayIndex)
		{
			this.Messages.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę wiadomości.
		/// </summary>
		public int Count
		{
			get { return this.Messages.Count; }
		}

		/// <summary>
		/// Zawsze false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IMessagesCollection Members
		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wiadomość o wskazanym typie.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Contains(MessageType type)
		{
			return this.Messages.FindIndex(m => m.Type == type) != -1;
		}

		/// <summary>
		/// Pobiera indeks wiadomości o wskazanym typie lub zwraca -1.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public int IndexOf(MessageType type)
		{
			return this.Messages.FindIndex(m => m.Type == type);
		}
		#endregion

		#region IEnumerable<Message> Members
		public IEnumerator<Message> GetEnumerator()
		{
			return this.Messages.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Messages.GetEnumerator();
		}
		#endregion

		#region Internal methods
		internal void InternalAdd(Message msg)
		{
			this.Messages.Add(msg);
		}
		#endregion
	}
}
