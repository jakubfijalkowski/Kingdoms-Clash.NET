using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Units;

	/// <summary>
	/// Kolekcja jednostek. Publicznie dostępna tylko do odczytu.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class UnitDescriptionsCollection
		: IUnitDescriptionsCollection
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IUnitDescription> Descriptions;
		#endregion

		#region IUnitDescriptionsCollection Members
		/// <summary>
		/// Wyszukuje jednostki na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Opis jednostki bądź null, gdy nie znaleziono.</returns>
		public IUnitDescription this[string id]
		{
			get { return this.Find(id); }
		}

		/// <summary>
		/// Wyszukuje jednostki na podstawie id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Opis jednostki bądź null, gdy nie znaleziono.</returns>
		public IUnitDescription Find(string id)
		{
			return this.Descriptions.Find(ud => ud.Id == id);
		}
		#endregion

		#region ICollection<IUnitDescription> Members
		/// <summary>
		/// Nieobsługiwane.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IUnitDescription item)
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
		/// Nieobsługiwane.
		/// </summary>
		/// <param name="item"></param>
		public bool Remove(IUnitDescription item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.Descriptions.RemoveAll(ud => ud.Id == item.Id) > 0;
		}

		/// <summary>
		/// Nieobsługiwane.
		/// </summary>
		/// <param name="item"></param>
		public void Clear()
		{
			this.Descriptions.Clear();
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera opis o id identycznym jak wskazany.
		/// </summary>
		/// <param name="item">Opis do porównania z.</param>
		/// <returns>Czy taki opis znajduje się w kolekcji.</returns>
		public bool Contains(IUnitDescription item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this[item.Id] != null;
		}

		/// <summary>
		/// Kopiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica.</param>
		/// <param name="arrayIndex">Indeks początkowy.</param>
		public void CopyTo(IUnitDescription[] array, int arrayIndex)
		{
			this.Descriptions.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba opisów w kolekcji.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Count
		{
			get { return this.Descriptions.Count; }
		}

		/// <summary>
		/// Czy kolekcja jest tylko do odczytu. Zawsze prawda.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IUnitDescription> Members
		public IEnumerator<IUnitDescription> GetEnumerator()
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

		#region Constructors
		/// <summary>
		/// Inicjalizuje nową, pustą kolekcje.
		/// </summary>
		public UnitDescriptionsCollection()
		{
			this.Descriptions = new List<IUnitDescription>();
		}

		/// <summary>
		/// Inicjalizuje nową kolekcje i dodaje do niej wksazane elementy.
		/// </summary>
		/// <param name="items">Elementy do dodania.</param>
		public UnitDescriptionsCollection(IEnumerable<IUnitDescription> items)
		{
			this.Descriptions = new List<IUnitDescription>(items);
		}
		#endregion
	}
}
