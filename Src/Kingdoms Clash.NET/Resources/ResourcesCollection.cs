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
		public void Add(string key, uint value)
		{
			this.Add(new KeyValuePair<string, uint>(key, value));
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
			if (!this.Resources.TryGetValue(key, out value))
			{
				value = 0;
			}
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
				if (this.Resources.ContainsKey(key))
				{
					return this.Resources[key];
				}
				return 0;
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
		public void Add(KeyValuePair<string, uint> item)
		{
			if (string.IsNullOrWhiteSpace(item.Key))
			{
				throw new ArgumentNullException("item.Key");
			}

			if (this.Resources.ContainsKey(item.Key))
			{
				this.Resources[item.Key] += item.Value;
			}
			else
			{
				this.Resources.Add(item.Key, item.Value);
			}
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
		public bool Contains(KeyValuePair<string, uint> item)
		{
			if (string.IsNullOrWhiteSpace(item.Key))
			{
				throw new ArgumentNullException("item.Key");
			}
			uint val;
			return this.Resources.TryGetValue(item.Key, out val) && val >= item.Value;
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
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Usuwa wskazaną ilość zasobu.
		/// </summary>
		/// <param name="item">Zasób.</param>
		/// <returns>Czy usunięto(czy znajdował się w kolekcji).</returns>
		public bool Remove(KeyValuePair<string, uint> item)
		{
			if (this.Resources.ContainsKey(item.Key))
			{
				this.Resources[item.Key] -= item.Value;
				//if (this.Resources[item.Key] < item.Value)
				//{
				//    throw new ArgumentException("Insufficient resources", "item");
				//}
				return true;
			}
			return false;
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
	}
}
