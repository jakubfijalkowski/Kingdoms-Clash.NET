using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Kolekcja atrybutów encji.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	class AttributesCollection
		: IAttributesCollection
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IAttribute> Attributes = new List<IAttribute>();
		private IGameEntity Parent = null;

		#region IAttributesCollection Members
		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		public IAttribute this[string id]
		{
			get { return this.Attributes.Find(a => a.Id == id); }
		}

		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <typeparam name="T">Typ atrybutu.</typeparam>
		/// <exception cref="System.InvalidCastException">Rzucane gdy atrybut istnieje i ma inny typ niż rządany.</exception>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		public IAttribute<T> Get<T>(string id)
		{
			var attr = this[id];
			if (attr == null || attr is IAttribute<T>)
			{
				return attr as IAttribute<T>;
			}
			throw new InvalidCastException(string.Format("Cannot cast attribute {0} to type {1}", id, typeof(T).ToString()));
		}

		/// <summary>
		/// Wyszukuje albo tworzy atrybut o podanym ID i typie.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Atrybut.</returns>
		public IAttribute GetOrCreate(string id)
		{
			var attr = this[id];
			if (attr == null)
			{
				attr = new Attribute(id, null);
				this.Attributes.Add(attr);
			}
			return attr;
		}

		/// <summary>
		/// Wyszukuje albo tworzy atrybut o podanym ID i typie.
		/// </summary>
		/// <typeparam name="T">Wymagany typ atrybutu.</typeparam>
		/// <param name="id">Identyfikator.</param>
		/// <exception cref="System.InvalidCastException">Rzucane gdy atrybut istnieje ale ma inny typ niż rządany.</exception>
		/// <returns>Atrybut.</returns>
		public IAttribute<T> GetOrCreate<T>(string id)
		{
			var attr = this.Get<T>(id);
			if (attr == null)
			{
				attr = new Attribute<T>(id, default(T));
				this.Attributes.Add(attr);
			}
			return attr;
		}

		/// <summary>
		/// Podmienia atrybut o podanym id na wskazany.
		/// Gdy atrybut nie istnieje - dodaje go.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="with">Atrybut zamieniany. Jeśli jest null to atrybut zostanie usunięty.</param>
		public void Replace(string id, IAttribute with)
		{
			var i = this.Attributes.FindIndex(a => a.Id == id);
			if (i == -1)
			{
				this.Attributes.Add(with);
			}
			else if (with == null)
			{
				this.Attributes.RemoveAt(i);
			}
			else
			{
				this.Attributes[i] = with;
			}
		}
		#endregion

		#region ICollection<IAttribute> Members
		/// <summary>
		/// Dodaje atrybut do encji.
		/// </summary>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane gdy taki atrybut został już dodany.</exception>
		/// <param name="attribute">Atrybut. Musi być unikatowy.</param>
		public void Add(IAttribute item)
		{
			if (this.Attributes.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Attributes.Add(item);
			Logger.Trace("Attribute {0} added to entity {1}", item.Id, this.Parent.Id);
		}

		/// <summary>
		/// Usuwa wskazany atrybut.
		/// </summary>
		/// <param name="item">Atrybut.</param>
		/// <returns>True jeśli usunięto, w przeciwnym razie false.</returns>
		public bool Remove(IAttribute item)
		{
			var deleted = this.Remove(item);
			if (deleted)
			{
				Logger.Debug("Attribute {0} removed from entity {1}", item.Id, this.Parent.Id);
			}
			return deleted;
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Attributes.Clear();
		}

		/// <summary>
		/// Sprawdza czy w kolekcji znajduje się wskazany atrybut.
		/// </summary>
		/// <param name="item">Atrybut.</param>
		/// <returns></returns>
		public bool Contains(IAttribute item)
		{
			return this.Attributes.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica.</param>
		/// <param name="arrayIndex">Początkowy indeks.</param>
		public void CopyTo(IAttribute[] array, int arrayIndex)
		{
			this.Attributes.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba atrybutów.
		/// </summary>
		public int Count
		{
			get { return this.Attributes.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion
		
		#region IEnumerable<IAttribute> Members
		public IEnumerator<IAttribute> GetEnumerator()
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

		internal AttributesCollection(IGameEntity parent)
		{
			this.Parent = parent;
		}
	}
}
