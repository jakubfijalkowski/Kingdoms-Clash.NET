using System.Collections.Generic;
using System.Linq;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Kolekcja komponentów.
	/// </summary>
	class ComponentsCollection
		: IComponentsCollection
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		private List<IComponent> Components = new List<IComponent>();
		private IGameEntity Parent = null;
		private RenderableComponentsCollection _RenderableComponentsCollection = new RenderableComponentsCollection();

		#region IComponentsCollection Members
		/// <summary>
		/// Lista komponentów które potrafią się renderować.
		/// </summary>
		public IRenderableComponentsCollection RenderableComponents
		{
			get { return this._RenderableComponentsCollection; }
		}

		/// <summary>
		/// Pobiera komponent o wskazanym id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Komponent lub null, gdy nie znaleziono.</returns>
		public IComponent this[string id]
		{
			get { return this.Components.Where(c => c.Id == id).FirstOrDefault(); }
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera komponent o wskazanym id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>True, jeśli zawiera, w przeciwnym razie false.</returns>
		public bool Contains(string id)
		{
			return this.Components.Find(c => c.Id == id) != null;
		}

		/// <summary>
		/// Sprawdza, czy kolekcja zawiera komponent o wskazanym type.
		/// </summary>
		/// <typeparam name="T">Typ komponentu.</typeparam>
		/// <returns>True, jeśli zawiera, w przeciwnym razie false</returns>
		public bool Contains<T>()
			where T : IComponent
		{
			return this.Components.Find(c => c is T) != null;
		}

		/// <summary>
		/// Usuwa komponenty o wskazanym id.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Ilość usuniętych komponentów.</returns>
		public int RemoveAll(string id)
		{
			this._RenderableComponentsCollection.InternalRemoveAll(id);
			return this.Components.RemoveAll(c => c.Id == id);
		}
		#endregion

		#region ICollection<IComponent> Members
		/// <summary>
		/// Dodaje komponent do kolekcji.
		/// Musi być unikatowy.
		/// </summary>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane gdy doadwany komponent już istnieje.</exception>
		/// <param name="item">Komponent.</param>
		public void Add(IComponent item)
		{
			if (this.Components.Contains(item))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("item");
			}
			this.Components.Add(item);
			item.Init(this.Parent);
			if(item is IRenderableComponent)
			{
				this._RenderableComponentsCollection.InternalAdd(item as IRenderableComponent);
			}
			Logger.Debug("Component {0} added to entity {1}", item.Id, this.Parent.Id);
		}

		/// <summary>
		/// Usuwa komponent z kolekcji.
		/// </summary>
		/// <param name="item">Element do usunięcia.</param>
		/// <returns>True jeśli usunięto komponent, w przeciwnym razie false.</returns>
		public bool Remove(IComponent item)
		{
			var deleted = this.Components.Remove(item);
			if (deleted)
			{
				if (item is IRenderableComponent)
				{
					this._RenderableComponentsCollection.InternalRemove(item as IRenderableComponent);
				}
				Logger.Debug("Component {0} removed from entity {1}", item.Id, this.Parent.Id);
			}
			return deleted;
		}

		/// <summary>
		/// Czyści listę komponentów.
		/// </summary>
		public void Clear()
		{
			this._RenderableComponentsCollection.InternalClear();
			this.Components.Clear();
		}

		/// <summary>
		/// Sprawdza, czy w kolekcji znajduje się dokładnie ten komponent.
		/// </summary>
		/// <param name="item">Komponent.</param>
		/// <returns>True, jeśli tak, w przeciwnym razie false.</returns>
		public bool Contains(IComponent item)
		{
			return this.Components.Contains(item);
		}

		/// <summary>
		/// Kompiuje kolekcje do tablicy.
		/// </summary>
		/// <param name="array">Tablica wyjściowa.</param>
		/// <param name="arrayIndex">Indekst początkowy.</param>
		public void CopyTo(IComponent[] array, int arrayIndex)
		{
			this.Components.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Liczba elementów.
		/// </summary>
		public int Count
		{
			get { return this.Components.Count; }
		}

		/// <summary>
		/// Czy lista jest tylko do odczytu.
		/// Zawsze false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region IEnumerable<IComponent> Members
		public IEnumerator<IComponent> GetEnumerator()
		{
			return this.Components.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Components.GetEnumerator();
		}
		#endregion

		internal ComponentsCollection(IGameEntity parent)
		{
			this.Parent = parent;
		}

		private class RenderableComponentsCollection
			: IRenderableComponentsCollection
		{
			private List<IRenderableComponent> Components = new List<IRenderableComponent>();

			#region ICollection<IRenderableComponent> Members
			public void Add(IRenderableComponent item)
			{
				throw new System.NotSupportedException();
			}

			public bool Remove(IRenderableComponent item)
			{
				throw new System.NotSupportedException();
			}

			public void Clear()
			{
				throw new System.NotSupportedException();
			}

			public bool Contains(IRenderableComponent item)
			{
				return this.Components.Contains(item);
			}

			public void CopyTo(IRenderableComponent[] array, int arrayIndex)
			{
				this.Components.CopyTo(array, arrayIndex);
			}

			public int Count
			{
				get { return this.Components.Count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}
			#endregion

			#region IEnumerable<IRenderableComponent> Members
			public IEnumerator<IRenderableComponent> GetEnumerator()
			{
				return this.Components.GetEnumerator();
			}
			#endregion

			#region IEnumerable Members
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.Components.GetEnumerator();
			}
			#endregion

			#region Internals
			internal void InternalAdd(IRenderableComponent item)
			{
				this.Components.Add(item);
			}

			internal void InternalRemove(IRenderableComponent item)
			{
				this.Components.Remove(item);
			}

			internal void InternalClear()
			{
				this.Components.Clear();
			}

			internal int InternalRemoveAll(string id)
			{
				return this.Components.RemoveAll(c => c.Id == id);
			}
			#endregion
		}
	}
}
