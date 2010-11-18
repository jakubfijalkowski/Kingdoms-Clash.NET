using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;

	/// <summary>
	/// Kontener na kontrolki.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class ControlsCollection
		: IControlsCollection
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IControl> Controls = new List<IControl>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IUIData UIData = null;
		#endregion

		#region IControlsCollection Members
		/// <summary>
		/// Dodaje listę kontrolek do kolekcji.
		/// </summary>
		/// <param name="items">Lista.</param>
		public void AddRange(IEnumerable<IControl> items)
		{
			foreach (var item in items)
			{
				if (this.Contains(item.Id))
				{
					throw new Exceptions.ArgumentAlreadyExistsException("item");
				}
				item.Data = this.UIData;
			}
			this.Controls.AddRange(items);
		}

		/// <summary>
		/// Usuwa kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Czy udało się usunąć kontrolkę.</returns>
		public bool Remove(string id)
		{
			return this.Controls.RemoveAll(c => c.Id == id) == 1;
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się kontrolka o podanym Id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public bool Contains(string id)
		{
			return this.Controls.Find(c => c.Id == id) != null;
		}

		/// <summary>
		/// Pobiera kontrolkę o wskazanym Id.
		/// </summary>
		/// <param name="index">Id.</param>
		/// <returns>Kontrolka, lub null, gdy nie znaleziono.</returns>
		public IControl this[string id]
		{
			get { return this.Controls.Find(c => c.Id == id); }
		}
		#endregion

		#region ICollection<IControl> Members
		/// <summary>
		/// Dodaje element do kolekcji.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Element o podanym Id już znajduje się w kolekcji.</exception>
		public void Add(IControl item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.Contains(item.Id))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			item.Data = this.UIData;
			this.Controls.Add(item);
		}

		/// <summary>
		/// Usuwa element o Id równym podanemu.
		/// </summary>
		/// <param name="item">Element do porównania.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		bool ICollection<IControl>.Remove(IControl item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Remove(item.Id);
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Controls.Clear();
		}

		/// <summary>
		/// Sprawdza, czy element o Id równym podanemu znajduje się w kolekcji.
		/// </summary>
		/// <param name="item">Element do porównania.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		bool ICollection<IControl>.Contains(IControl item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Contains(item.Id);
		}

		/// <summary>
		/// Kopiuje elementy kolekcji do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<IControl>.CopyTo(IControl[] array, int arrayIndex)
		{
			this.Controls.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba elementów w kolekcji.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count { get { return this.Controls.Count; } }

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		bool ICollection<IControl>.IsReadOnly { get { return false; } }
		#endregion

		#region IEnumerable<IControl> Members
		public IEnumerator<IControl> GetEnumerator()
		{
			return this.Controls.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Controls.GetEnumerator();
		}
		#endregion

		#region Constructors
		public ControlsCollection(IUIData uiData = null)
		{
			this.UIData = uiData;
		}
		#endregion
	}
}
