using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Manager encji gry.
	/// </summary>
	public class EntitiesManager
		: IEntitiesManager
	{
		#region Properties
		private List<IGameEntity> Entities_ = new List<IGameEntity>();

		/// <summary>
		/// Lista encji.
		/// </summary>
		public ReadOnlyCollection<IGameEntity> Entities
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
		public void AddEntity(IGameEntity entity)
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
			entity.Init(this);
		}

		/// <summary>
		/// Usuwa encję.
		/// </summary>
		/// <param name="entity">Encja do usunięcia.</param>
		public void RemoveEntity(IGameEntity entity)
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
