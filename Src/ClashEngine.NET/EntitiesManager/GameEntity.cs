using System;
using System.Collections.Generic;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Encja gry - kontener na komponenty i atrybuty.
	/// </summary>
	public class GameEntity
		: IGameEntity
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		private List<IComponent> _Components = new List<IComponent>();
		private List<IRenderableComponent> _RenderableComponents = new List<IRenderableComponent>();
		private IAttributesCollection _Attributes;

		#region Properties
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Manager encji.
		/// </summary>
		public IEntitiesManager Manager { get; private set; }

		/// <summary>
		/// Lista komponentów.
		/// Zmieniać za pomocą odpowiednich metod, nie ręcznie!
		/// </summary>
		public IList<IComponent> Components
		{
			get { return this._Components; }
		}

		/// <summary>
		/// Kolekcja atrybutów.
		/// </summary>
		public IAttributesCollection Attributes
		{
			get { return this._Attributes; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje nową encję.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public GameEntity(string id)
		{
			Logger.Trace("Creating new game entity with id {0}", id);
			this._Attributes = new AttributesCollection(this);
			this.Id = id;
		}

		/// <summary>
		/// Wywoływane przy inicjalizacji encji gry - dadaniu do managera.
		/// </summary>
		/// <param name="entitiesManager">Właściciel encji.</param>
		public void Init(IEntitiesManager entitiesManager)
		{
			this.Manager = entitiesManager;
			this.InitEntity();
			Logger.Debug("Game entity {0} initialized", this.Id);
		}

		/// <summary>
		/// Dodaje komponent do encji.
		/// </summary>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane gdy taki komponent został już dodany.</exception>
		/// <param name="component">Komponent. Musi być unikatowy.</param>
		public void AddComponent(IComponent component)
		{
			if (this._Components.Contains(component))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("component");
			}
			this._Components.Add(component);
			if (component is RenderableComponent)
			{
				this._RenderableComponents.Add(component as RenderableComponent);
			}
			component.Init(this);
			Logger.Trace("Component {0} added to entity {1}", component.Id, this.Id);
		}

		/// <summary>
		/// Uaktualnia wszystkie komponenty.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public virtual void Update(double delta)
		{
			foreach (IComponent c in this._Components)
			{
				c.Update(delta);
			}
		}

		/// <summary>
		/// Rysuje encję.
		/// </summary>
		public virtual void Render()
		{
			foreach (IRenderableComponent c in this._RenderableComponents)
			{
				c.Render();
			}
		}
		#endregion

		#region Other
		/// <summary>
		/// Inicjalizacja encji - wywoływane po inicjalizacji encji w managerze(metoda Init).
		/// </summary>
		public virtual void InitEntity()
		{ }
		#endregion
	}
}
