using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Units;

	/// <summary>
	/// Kolekcja atrybutów jednostek.
	/// Wsyzstkie porównywania są prowadzone po Id, więc nie mogą istnieć dwa elementy o tym samym Id.
	/// </summary>
	public class UnitAttributesCollection
		: IUnitAttributesCollection, ICollection<IUnitAttribute>
	{
		private List<IUnitAttribute> Attributes = new List<IUnitAttribute>();

		#region IUnitAttributesCollection Members
		/// <summary>
		/// Pobiera atrybut o wskazanym identyfikatorze.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Wartość atrybutu lub null, gdy nie odnaleziono.</returns>
		public object this[string id]
		{
			get
			{
				var attr = this.Attributes.Find(ua => ua.Id == id);
				if (attr != null)
				{
					return attr.Value;
				}
				return null;
			}
		}

		/// <summary>
		/// Pobiera atrybut o wskazanym identyfikatorze.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Wartość atrybutu lub null, gdy nie odnaleziono.</returns>
		public object Get(string id)
		{
			var attr = this.Attributes.Find(ua => ua.Id == id);
			if (attr != null)
			{
				return attr.Value;
			}
			return null;
		}

		/// <summary>
		/// Pobiera atrybut o wskazanym identyfikatorze.
		/// </summary>
		/// <typeparam name="T">Rządany typ atrybutu.</typeparam>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Wartość atrybutu lub domyślną wartość dla T, gdy nie odnaleziono.</returns>
		public T Get<T>(string id)
		{
			var attr = this.Attributes.Find(ua => ua.Id == id);
			if (attr != null)
			{
				return (T)attr.Value;
			}
			return default(T);
		}
		#endregion

		#region ICollection<IUnitAttribute> Members
		/// <summary>
		/// Dodaje atrybut do kolekcji.
		/// Musi posiadać unikatowe Id.
		/// </summary>
		/// <param name="item">Atrybut.</param>
		/// <exception cref="ClashEngine.NET.Exceptions.ArgumentAlreadyExistsException">Rzucane, gdy atrybut o podanym Id już istnieje.</exception>
		public void Add(IUnitAttribute item)
		{
			if (this.Contains(item))
			{
				throw new ClashEngine.NET.Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Attributes.Add(item);
		}

		/// <summary>
		/// Usuwa atrybut o Id identycznym jak podany atrybut.
		/// </summary>
		/// <param name="item">Atrybut do porównania.</param>
		/// <returns></returns>
		public bool Remove(IUnitAttribute item)
		{
			return this.Attributes.RemoveAll(ua => ua.Id == item.Id) > 0;
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Attributes.Clear();
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera atrybut o id takim samym jak podany.
		/// </summary>
		/// <param name="item">Atrybut do porównania.</param>
		/// <returns></returns>
		public bool Contains(IUnitAttribute item)
		{
			return this.Attributes.Find(ua => ua.Id == item.Id) != null;
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica.</param>
		/// <param name="arrayIndex">Początkowy indeks.</param>
		public void CopyTo(IUnitAttribute[] array, int arrayIndex)
		{
			this.Attributes.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba zapisanych atrybutów.
		/// </summary>
		public int Count
		{
			get { return this.Attributes.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu.
		/// Zawsze false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IUnitAttribute> Members
		public IEnumerator<IUnitAttribute> GetEnumerator()
		{
			return this.Attributes.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Attributes.GetEnumerator();
		}
		#endregion
	}
}
