using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Manager encji gry.
	/// </summary>
	[DebuggerDisplay("Count = {Count}")]
	public class EntitiesManager
		: IEntitiesManager
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private List<IGameEntity> Entities = new List<IGameEntity>();

		#region IEntitiesManager Members
		/// <summary>
		/// Uaktualnia eszystkie encje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			//Zastosowanie for pozwoli na modyfikacje(dodanie na koniec) kolekcji w trakcie działania.
			for (int i = 0; i < this.Entities.Count; i++)
			{
				this.Entities[i].Update(delta);
			}
		}

		/// <summary>
		/// Renderuje wszystkie renderowalne komponenty encji.
		/// </summary>
		public void Render()
		{
			foreach (IGameEntity entity in this.Entities)
			{
				entity.Render();
			}
		}
		#endregion

		#region ICollection<IGameEntity> Members
		/// <summary>
		/// Liczba encji.
		/// </summary>
		public int Count
		{
			get { return this.Entities.Count; }
		}

		/// <summary>
		/// Czy jest tylko do odczytu.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Dodaje encję.
		/// Nie można dodawać dwóch IDENTYCZNYCH(ten sam obiekt) encji - wiele encji o tym samym ID jest dozwolone.
		/// </summary>
		/// <param name="entity">Encja do dodania.</param>
		public void Add(IGameEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			else if (this.Entities.Contains(entity))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("entity");
			}
			entity.Init(this);
			this.Entities.Add(entity);
			entity.OnInit();
			Logger.Trace("Entity {0} added to manager", entity.Id);
		}

		/// <summary>
		/// Usuwa encję.
		/// </summary>
		/// <param name="entity">Encja do usunięcia.</param>
		public bool Remove(IGameEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			var deleted = this.Entities.Remove(entity);
			if (deleted)
			{
				Logger.Trace("Entity {0} removed from manager", entity.Id);
			}
			entity.OnDeinit();
			return deleted;
		}

		/// <summary>
		/// Usuwa wszystkie encje z kolekcji.
		/// </summary>
		public void Clear()
		{
			foreach (var e in this.Entities)
			{
				e.OnDeinit();
			}
			this.Entities.Clear();
		}
		
		/// <summary>
		/// Sprawdza, czy kolekcja zawiera daną encję.
		/// </summary>
		/// <param name="item">Encja.</param>
		/// <returns></returns>
		public bool Contains(IGameEntity item)
		{
			return this.Entities.Contains(item);
		}

		/// <summary>
		/// Kopiuje encje do tablicy.
		/// </summary>
		/// <param name="array">Tablica wyjściowa.</param>
		/// <param name="arrayIndex">Początkowy indeks.</param>
		public void CopyTo(IGameEntity[] array, int arrayIndex)
		{
			this.Entities.CopyTo(array, arrayIndex);
		}
		#endregion

		#region IEnumerable<IGameEntity> Members
		public IEnumerator<IGameEntity> GetEnumerator()
		{
			return this.Entities.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Entities.GetEnumerator();
		}
		#endregion

		~EntitiesManager()
		{
			this.Clear();
		}
	}
}
