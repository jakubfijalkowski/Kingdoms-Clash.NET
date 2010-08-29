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
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		private List<IGameEntity> _Entities = new List<IGameEntity>();

		#region Properties
		/// <summary>
		/// Lista encji.
		/// </summary>
		public ReadOnlyCollection<IGameEntity> Entities
		{
			get { return this._Entities.AsReadOnly(); }
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
			else if (this._Entities.Contains(entity))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("entity");
			}
			entity.Init(this);
			this._Entities.Add(entity);
			Logger.Info("Entity {0} added to manager", entity.Id);
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
			else if (!this._Entities.Contains(entity))
			{
				throw new Exceptions.ArgumentNotExistsException("entity");
			}
			this._Entities.Remove(entity);
			Logger.Info("Entity {0} removed from manager", entity.Id);
		}

		/// <summary>
		/// Uaktualnia eszystkie encje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			foreach (IGameEntity entity in this._Entities)
			{
				entity.Update(delta);
			}
		}

		/// <summary>
		/// Renderuje wszystkie renderowalne komponenty encji.
		/// </summary>
		public void Render()
		{
			foreach (IGameEntity entity in this._Entities)
			{
				entity.Render();
			}
		}
		#endregion
	}
}
