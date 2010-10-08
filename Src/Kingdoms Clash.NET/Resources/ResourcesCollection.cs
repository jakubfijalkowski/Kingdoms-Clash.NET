using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Resources
{
	using Interfaces.Resources;

	/// <summary>
	/// Kolekcja zasobów.
	/// </summary>
	/// <remarks>
	/// Nie wspiera usuwania zasobów przez IDictionary.Remove ani kopiowania do tablicy(ICollection.CopyTo).
	/// Usuwanie zasobu zakłada, że go wystarczy - nie sprawdza przepełnienia liczby.
	/// </remarks>
	public class ResourcesCollection
		: IResourcesCollection
	{
		private Dictionary<string, uint> Resources = new Dictionary<string, uint>();

		#region IDictionary<string,uint> Members
		/// <summary>
		/// Dodaje zasób.
		/// </summary>
		/// <param name="key">Identyfikator zasobu.</param>
		/// <param name="value">Wartość do dodania.</param>
		/// <exception cref="ArgumentNullException">Rzucane, gdy klucz jest nullem lub jest pusty.</exception>
		public void Add(string key, uint value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException("item.Key");
			}

			if (this.Resources.ContainsKey(key))
			{
				this.Resources[key] += value;
			}
			else
			{
				this.Resources.Add(key, value);
			}
		}

		/// <summary>
		/// Sprawdzamy, czy w kolekcji w ogóle znajduje się zasób.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IDictionary<string, uint>.ContainsKey(string key)
		{
			return this.Resources.ContainsKey(key);
		}

		/// <summary>
		/// Niewspierane - używać Remove(KeyValuePair&lt;string, uint&gt;).
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IDictionary<string, uint>.Remove(string key)
		{
			throw new NotSupportedException("Not supported, use Remove(KeyValuePair<string, uint>) instead");
		}

		/// <summary>
		/// Pobiera wartość zasobu o wskazanym id.
		/// Nie ma prawa się nie powieść. Gdy nie ma zasobu zwraca 0.
		/// </summary>
		/// <param name="key">Identyfikator zasobu.</param>
		/// <param name="value">Wyjściowa wartość.</param>
		/// <returns>Zawsze true.</returns>
		public bool TryGetValue(string key, out uint value)
		{
			value = 0;
			this.Resources.TryGetValue(key, out value);
			return true;
		}

		/// <summary>
		/// Kolekcja identyfikatorów zasobów.
		/// </summary>
		ICollection<string> IDictionary<string, uint>.Keys
		{
			get { return this.Resources.Keys; }
		}

		/// <summary>
		/// Kolekcja ich wartości.
		/// </summary>
		ICollection<uint> IDictionary<string, uint>.Values
		{
			get { return this.Resources.Values; }
		}

		/// <summary>
		/// Pobiera wartość zasobu o wskazanym Id.
		/// Zmiana jest niewspierana.
		/// Gdy zasób nieistnieje - zwraca 0.
		/// </summary>
		/// <param name="key">Indetyfikator zasobu.</param>
		/// <returns></returns>
		public uint this[string key]
		{
			get
			{
				uint value = 0;
				this.Resources.TryGetValue(key, out value);
				return value;
			}
			set
			{
				throw new NotSupportedException("Not supported, use Add or Remove instead");
			}
		}
		#endregion

		#region ICollection<KeyValuePair<string,uint>> Members
		/// <summary>
		/// Dodaje wskazany zasób.
		/// </summary>
		/// <param name="item">Zasób.</param>
		/// <exception cref="ArgumentNullException">Rzucane, gdy klucz jest nullem lub jest pusty.</exception>
		void ICollection<KeyValuePair<string, uint>>.Add(KeyValuePair<string, uint> item)
		{
			this.Add(item.Key, item.Value);
		}

		/// <summary>
		/// Czyści kolekcję zasobów do zera.
		/// </summary>
		public void Clear()
		{
			this.Resources.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wystarczająca ilość zasobu.
		/// </summary>
		/// <param name="item">Zasób.</param>
		/// <exception cref="ArgumentNullException">Rzucane, gdy klucz jest nullem lub jest pusty.</exception>
		/// <returns>Czy jest go wystarczająco.</returns>
		bool ICollection<KeyValuePair<string, uint>>.Contains(KeyValuePair<string, uint> item)
		{
			return this.Contains(item.Key, item.Value);
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<KeyValuePair<string, uint>>.CopyTo(KeyValuePair<string, uint>[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Zwraca liczbę posiadanych typów zasobów.
		/// </summary>
		public int Count
		{
			get { return this.Resources.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
		/// </summary>
		bool ICollection<KeyValuePair<string, uint>>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Usuwa wskazaną ilość zasobu.
		/// </summary>
		/// <param name="item">Zasób.</param>
		/// <returns>Czy udało się usunąć.</returns>
		bool ICollection<KeyValuePair<string, uint>>.Remove(KeyValuePair<string, uint> item)
		{
			return this.Remove(item.Key, item.Value);
		}
		#endregion

		#region IEnumerable<KeyValuePair<string,uint>> Members
		public IEnumerator<KeyValuePair<string, uint>> GetEnumerator()
		{
			return this.Resources.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Resources.GetEnumerator();
		}
		#endregion

		#region IResourcesCollection Members
		/// <summary>
		/// Usuwa z kolekcji podaną ilość zasobu.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <param name="value">Wartość.</param>
		public bool Remove(string id, uint value)
		{
			if (this.Resources.ContainsKey(id))
			{
				this.Resources[id] -= value;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się wskazana ilość danego zasobu.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <param name="value">Żądana ilość.</param>
		/// <returns>Czy jest go wystarczająco.</returns>
		public bool Contains(string id, uint value)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentNullException("id");
			}
			uint val;
			return this.Resources.TryGetValue(id, out val) && val >= value;
		}
		#endregion
	}
}
