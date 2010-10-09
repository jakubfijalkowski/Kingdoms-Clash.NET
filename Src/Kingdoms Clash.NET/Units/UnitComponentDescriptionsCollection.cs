using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Units;

	/// <summary>
	/// Kolekcja opisów komponentów jednostek.
	/// W kolekcji nie mogą znajdować się dwa opisy o takim samym typie.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class UnitComponentDescriptionsCollection
		: IUnitComponentDescriptionsCollection
	{
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IUnitComponentDescription> Descriptions = new List<IUnitComponentDescription>();

		#region IUnitComponentDescriptionsCollection Members
		/// <summary>
		/// Pobiera listę opisów o typie dziedziczącym ze wskazanego.
		/// </summary>
		/// <param name="type">Typ.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		/// <returns>Lista komponentów.</returns>
		public IEnumerable<IUnitComponentDescription> Get(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			else if (type.IsInterface)
			{
				return this.Descriptions.Where(uc => uc.GetType().GetInterfaces().Any(t => t == type));
			}
			else
			{
				return this.Descriptions.Where(uc => uc.GetType().IsSubclassOf(type));
			}
		}

		/// <summary>
		/// Pobiera listę opisów o typie dziedziczącym ze wskazanego.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <returns>Lista komponentów.</returns>
		public IEnumerable<T> Get<T>()
			where T : IUnitComponentDescription
		{
			return this.Descriptions.Where(uc => uc is T).Cast<T>();
		}

		/// <summary>
		/// Pobiera opis komponentu o wskazanym typie.
		/// </summary>
		/// <param name="type">Typ.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		public IUnitComponentDescription GetSingle(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			else if(type.IsInterface)
			{
				throw new ArgumentException("Cannot be interface", "type");
			}
			return this.Descriptions.Where(uc => uc.GetType().IsSubclassOf(type)).FirstOrDefault();
		}

		/// <summary>
		/// Pobiera opis komponentu o wskazanym typie.
		/// </summary>
		/// <typeparam name="T">Typ.</typeparam>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		public T GetSingle<T>()
			where T : class, IUnitComponentDescription
		{
			return this.Descriptions.Where(uc => uc is T).FirstOrDefault() as T;
		}
		#endregion

		#region ICollection<IUnitComponentDescription> Members
		/// <summary>
		/// Dodaje opis do kolekcji.
		/// </summary>
		/// <param name="item">Opis.</param>
		/// <exception cref="ClashEngine.NET.Exceptions.ArgumentAlreadyExistsException">Rzucane, gdy w kolekcji już znajduje się wskazany opis.</exception>
		/// <exception cref="ArgumentNullException">Rzucane gdy item == null.</exception>
		public void Add(IUnitComponentDescription item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			else if (this.Contains(item))
			{
				throw new ClashEngine.NET.Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Descriptions.Add(item);
		}

		/// <summary>
		/// Usuwa wskazany opis.
		/// </summary>
		/// <param name="item">Opis.</param>
		/// <returns>Czy usunięto.</returns>
		/// <exception cref="ArgumentNullException">Rzucane gdy item == null.</exception>
		public bool Remove(IUnitComponentDescription item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Descriptions.Remove(item);
		}

		/// <summary>
		/// Czyści kolekcję.
		/// </summary>
		public void Clear()
		{
			this.Descriptions.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się dokładnie ten opis.
		/// </summary>
		/// <param name="item">Opis.</param>
		/// <returns>True, gdy prawda, inaczej false.</returns>
		/// <exception cref="ArgumentNullException">Rzucane gdy item == null.</exception>
		public bool Contains(IUnitComponentDescription item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Descriptions.Contains(item);
		}

		/// <summary>
		/// Kopiuje kolekcję do tablicy.
		/// </summary>
		/// <param name="array">Tablica.</param>
		/// <param name="arrayIndex">Indeks początkowy.</param>
		public void CopyTo(IUnitComponentDescription[] array, int arrayIndex)
		{
			this.Descriptions.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Pobiera liczbę elementów kolekcji.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count
		{
			get { return this.Descriptions.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu - zawsze false.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IUnitComponentDescription> Members
		public IEnumerator<IUnitComponentDescription> GetEnumerator()
		{
			return this.Descriptions.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Descriptions.GetEnumerator();
		}
		#endregion
	}
}
