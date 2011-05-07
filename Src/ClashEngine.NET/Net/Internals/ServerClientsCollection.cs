using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Net.Internals
{
	using Collections;
	using Interfaces.Net;

	/// <summary>
	/// Wewnętrzna kolekcja klientów.
	/// </summary>
	internal class ServerClientsCollection
		: SafeList<IClient>, IClientsCollection
	{
		#region IList<IClient> Members
		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		new public void Insert(int index, IClient item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Usuwa klienta z kolekcji i zamyka połączenie.
		/// </summary>
		/// <param name="index"></param>
		new public void RemoveAt(int index)
		{
			try
			{
				this.RWLock.EnterWriteLock();
				this.InnerList[index].Close();
				this.InnerList.RemoveAt(index);
			}
			finally
			{
				this.RWLock.ExitWriteLock();
			}
		}

		/// <summary>
		/// Pobiera element ze wskazanego miejsca.
		/// Zmiana elementu jest niewspierana.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		new public IClient this[int index]
		{
			get { return base[index]; }
			set { throw new NotSupportedException(); }
		}
		#endregion

		#region ICollection<IClient> Members
		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="item"></param>
		new public void Add(IClient item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Usuwa klienta z kolekcji.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		new public bool Remove(IClient item)
		{
			bool ret = false;
			base.RWLock.EnterUpgradeableReadLock();
			int idx = base.InnerList.IndexOf(item);
			if (idx > -1)
			{
				base.RWLock.EnterWriteLock();
				base.InnerList[idx].Close();
				base.InnerList.RemoveAt(idx);
				ret = true;
				base.RWLock.ExitWriteLock();
			}
			base.RWLock.ExitUpgradeableReadLock();
			return ret;
		}

		/// <summary>
		/// Zamyka wszystkie połączenia i usuwa klientów z kolekcji.
		/// </summary>
		new public void Clear()
		{
			base.RWLock.EnterWriteLock();
			foreach (var client in base.InnerList)
			{
				client.Close();
			}
			base.InnerList.Clear();
			base.RWLock.ExitWriteLock();
		}
		#endregion

		#region IClientsCollection Members
		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów.
		/// </summary>
		/// <param name="msg"></param>
		public void SendToAll(Message msg)
		{
			foreach (var client in this)
			{
				if (client.Status == ClientStatus.Ok)
					client.Send(msg);
			}
		}

		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów umożliwiając ich filtorwanie.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		/// <param name="pred">Predykat.</param>
		public void SendToAll(Message msg, Predicate<IClient> pred)
		{
			foreach (var client in this)
			{
				if (client.Status == ClientStatus.Ok && pred(client))
					client.Send(msg);
			}
		}

		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów.
		/// Nie lockuje kolekcji.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		public void SendToAllNoLock(Message msg)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Status == ClientStatus.Ok)
					this[i].Send(msg);
			}
		}

		/// <summary>
		/// Wysyła wiadomość do wszystkich, "prawidłowych", klientów umożliwiając ich filtorwanie.
		/// Nie lockuje kolekcji.
		/// </summary>
		/// <param name="msg">Wiadomość</param>
		/// <param name="pred">Predykat.</param>
		public void SendToAllNoLock(Message msg, Predicate<IClient> pred)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Status == ClientStatus.Ok && pred(this[i]))
					this[i].Send(msg);
			}
		}
		#endregion

		#region Internals
		internal void InternalAdd(IClient item)
		{
			base.Add(item);
		}
		#endregion
	}
}
