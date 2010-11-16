using System;
using System.Collections.Generic;

namespace ClashEngine.NET.Graphics.Gui.Xaml
{
	using Interfaces.Graphics.Gui.Xaml;

	/// <summary>
	/// Kontener na kontrolki XAML.
	/// </summary>
	public class XamlControlsCollection
		: IXamlControlsCollection
	{
		private List<IXamlControl> Controls = new List<IXamlControl>();

		#region ICollection<IXamlControl> Members
		/// <summary>
		/// Dodaje element do kolekcji.
		/// </summary>
		/// <param name="item">Element.</param>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Element o podanym Id już znajduje się w kolekcji.</exception>
		public void Add(IXamlControl item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (this.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Controls.Add(item);
		}

		/// <summary>
		/// Usuwa element o Id równym podanemu.
		/// </summary>
		/// <param name="item">Element do porównania.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">item jest nullem.</exception>
		public bool Remove(IXamlControl item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Controls.RemoveAll(c => c.Id == item.Id) == 1;
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
		public bool Contains(IXamlControl item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Controls.Find(c => c.Id == item.Id) != null;
		}

		/// <summary>
		/// Kopiuje elementy kolekcji do tablicy.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<IXamlControl>.CopyTo(IXamlControl[] array, int arrayIndex)
		{
			this.Controls.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba elementów w kolekcji.
		/// </summary>
		public int Count { get { return this.Controls.Count; } }

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
		/// </summary>
		bool ICollection<IXamlControl>.IsReadOnly { get { return false; } }
		#endregion

		#region IEnumerable<IXamlControl> Members
		public IEnumerator<IXamlControl> GetEnumerator()
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
	}
}
