using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Kolekcja atrybutów encji.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	internal class AttributesCollection
		: KeyedCollection<string, IAttribute>, IAttributesCollection
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		private IGameEntity Parent = null;

		#region IAttributesCollection Members
		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <typeparam name="T">Typ atrybutu.</typeparam>
		/// <exception cref="System.InvalidCastException">Rzucane gdy atrybut istnieje i ma inny typ niż rządany.</exception>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		public IAttribute<T> Get<T>(string id)
		{
			var attr = this.Find(id);
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
			var attr = this.Find(id);
			if (attr == null)
			{
				attr = new Attribute(id, null);
				base.Add(attr);
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
				base.Add(attr);
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
			int index = -1;
			//index = this.Attributes.FindIndex(a => a.Id == id);
			for (int i = 0; i < this.Count; i++)
			{
				if (this.Comparer.Equals(id, base.Items[i].Id))
				{
					index = i;
					break;
				}
			}			
			if (index == -1)
			{
				base.Add(with);
			}
			else if (with == null)
			{
				base.RemoveAt(index);
			}
			else
			{
				base.SetItem(index, with);
			}
		}
		#endregion

		#region KeyedCollection Members
		protected override string GetKeyForItem(IAttribute item)
		{
			if (string.IsNullOrWhiteSpace(item.Id))
			{
				throw new ArgumentNullException("item.Id");
			}
			return item.Id;
		}

		protected override void InsertItem(int index, IAttribute item)
		{
			base.InsertItem(index, item);
			Logger.Trace("Attribute {0} added to entity {1}", item.Id, this.Parent.Id);
		}

		protected override void RemoveItem(int index)
		{
			Logger.Trace("Attribute {0} removed from entity {1}", this[index].Id, this.Parent.Id);
			base.RemoveItem(index);
		}
		#endregion

		#region Constructors
		internal AttributesCollection(IGameEntity parent)
		{
			this.Parent = parent;
		}
		#endregion

		#region Private methods
		private IAttribute Find(string id)
		{
			IAttribute attr = null;
			if (this.Count == 0 || !base.Dictionary.TryGetValue(id, out attr))
			{
				return null;
			}
			return attr;
		}
		#endregion
	}
}
