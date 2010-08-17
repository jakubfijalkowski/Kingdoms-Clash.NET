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
		private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
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
			throw new InvalidCastException("Cannot cast attribute to IAttribute<> with type " + typeof(T).ToString());
		}

		/// <summary>
		/// Uaktualnia wszystkie komponenty.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public virtual void Update(double delta)
		{
			foreach (Component c in this._Components)
			{
				c.Update(delta);
			}
		}

		/// <summary>
		/// Rysuje encję.
		/// </summary>
		public virtual void Render()
		{
			foreach (RenderableComponent c in this._RenderableComponents)
			{
				c.Render();
			}
		}
		#endregion
	}
}
