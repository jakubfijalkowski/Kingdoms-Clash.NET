using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
		private List<IAttribute> _Attributes = new List<IAttribute>();

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
		/// </summary>
		public ReadOnlyCollection<IComponent> Components
		{
			get { return this._Components.AsReadOnly(); }
		}

		/// <summary>
		/// Lista atrybutów.
		/// </summary>
		public ReadOnlyCollection<IAttribute> Attributes
		{
			get { return this._Attributes.AsReadOnly(); }
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
			this.Id = id;
		}

		/// <summary>
		/// Wywoływane przy inicjalizacji encji gry - dadaniu do managera.
		/// </summary>
		/// <param name="entitiesManager">Właściciel encji.</param>
		public void Init(IEntitiesManager entitiesManager)
		{
			this.Manager = entitiesManager;
			Logger.Info("Game entity {0} initialized", this.Id);
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
			Logger.Info("Component {0} added to entity {1}", component.Id, this.Id);
		}

		/// <summary>
		/// Dodaje atrybut do encji.
		/// </summary>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane gdy taki atrybut został już dodany.</exception>
		/// <param name="attribute">Atrybut. Musi być unikatowy.</param>
		public void AddAttribute(IAttribute attribute)
		{
			if (this._Attributes.Contains(attribute))
			{
				throw new Exceptions.ArgumentAlreadyExistsException("attribute");
			}
			this._Attributes.Add(attribute);
			Logger.Info("Attribute {0} added to entity {1}", attribute.Id, this.Id);
		}

		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		public IAttribute GetAttribute(string id)
		{
			return this._Attributes.Find((a) => a.Id == id);
		}

		/// <summary>
		/// Wyszukuje atrybutu po ID.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <typeparam name="T">Typ atrybutu.</typeparam>
		/// <returns>Atrybut lub null, gdy nie znaleziono.</returns>
		public IAttribute<T> GetAttribute<T>(string id)
		{
			IAttribute attr = this.GetAttribute(id);
			if (attr is IAttribute<T>)
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
		public IAttribute GetOrCreateAttribute(string id)
		{
			IAttribute attr = this.GetAttribute(id);
			if (attr == null)
			{
				attr = new Attribute(id, null);
				this.AddAttribute(attr);
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
		public IAttribute<T> GetOrCreateAttribute<T>(string id)
		{
			IAttribute attr = this.GetAttribute(id);
			if (attr == null)
			{
				IAttribute<T> a = new Attribute<T>(id, default(T));
				this.AddAttribute(a);
				return a;
			}
			else if(!(attr is IAttribute<T>))
			{
				throw new InvalidCastException(string.Format("Cannot cast attribute {0} to type {1}", id, typeof(T).ToString()));
			}
			return attr as IAttribute<T>;
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
	}
}
