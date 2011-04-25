using System;

namespace ClashEngine.NET.Net.Internals
{
	using Collections;
	using Interfaces.Net;

	/// <summary>
	/// Kolekcja wiadomości.
	/// </summary>
	internal class MessagesCollection
		: SafeList<Message>, IMessagesCollection
	{
		#region IList<Message> Members
		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		new public void Insert(int index, Message item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Pobiera element ze wskazanego miejsca.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		new public Message this[int index]
		{
			get { return base[index]; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		#region ICollection<Message> Members
		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		new public void Add(Message item)
		{
			throw new NotSupportedException();
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
			base.RWLock.EnterReadLock();
			var r = base.InnerList.FindIndex(m => m.Type == type) != -1;
			base.RWLock.ExitReadLock();
			return r;
		}

		/// <summary>
		/// Pobiera indeks wiadomości o wskazanym typie lub zwraca -1.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public int IndexOf(MessageType type)
		{
			base.RWLock.EnterReadLock();
			var r = base.InnerList.FindIndex(m => m.Type == type);
			base.RWLock.ExitReadLock();
			return r;
		}

		/// <summary>
		/// Pobiera pierwszą wiadomość o wskazanym typie.
		/// </summary>
		/// <param name="type">Typ wiadomości.</param>
		/// <returns></returns>
		public Message GetFirst(MessageType type)
		{
			base.RWLock.EnterReadLock();
			var r = base.InnerList.Find(m => m.Type == type);
			base.RWLock.ExitReadLock();
			return r;
		}
		#endregion

		#region Internal methods
		internal void InternalAdd(Message msg)
		{
			base.Add(msg);
		}
		#endregion
	}
}
