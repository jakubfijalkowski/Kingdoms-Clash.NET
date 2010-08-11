using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET.EntitiesManager
{
	public class EntitiesManager
	{
		#region Properties
		private List<GameEntity> Entities_ = new List<GameEntity>();

		/// <summary>
		/// Lista encji.
		/// </summary>
		public ReadOnlyCollection<GameEntity> Entities
		{
			get { return this.Entities_.AsReadOnly(); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Dodaje encję.
		/// Nie można dodawać dwóch IDENTYCZNYCH(ten sam obiekt) encji - wiele encji o tym samym ID jest dozwolone.
		/// </summary>
		/// <param name="entity">Encja do dodania.</param>
		public void AddEntity(GameEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			else if (this.Entities_.Contains(entity))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("entity");
			}
			this.Entities_.Add(entity);
		}

		/// <summary>
		/// Usuwa encję.
		/// </summary>
		/// <param name="entity">Encja do usunięcia.</param>
		public void RemoveEntity(GameEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			else if (!this.Entities_.Contains(entity))
			{
				throw new Exceptions.ArgumentNotExistsException("entity");
			}
			this.Entities_.Remove(entity);
		}

		/// <summary>
		/// Uaktualnia eszystkie encje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			foreach (GameEntity entity in this.Entities_)
			{
				entity.Update(delta);
			}
		}

		/// <summary>
		/// Renderuje wszystkie renderowalne komponenty encji.
		/// Do zaimplementowania.
		/// </summary>
		public void Render()
		{ }
		#endregion
	}
}
