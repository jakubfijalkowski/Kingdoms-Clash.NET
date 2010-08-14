using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET.EntitiesManager
{
	/// <summary>
	/// Encja gry - kontener na komponenty i atrybuty.
	/// </summary>
	public class GameEntity
	{
		private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private List<Component> _Components = new List<Component>();
		private List<RenderableComponent> _RenderableComponents = new List<RenderableComponent>();
		private List<Attribute> _Attributes = new List<Attribute>();

		#region Properties
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Manager encji.
		/// </summary>
		public EntitiesManager Manager { get; internal set; }

		/// <summary>
		/// Lista komponentów.
		/// </summary>
		public ReadOnlyCollection<Component> Components
		{
			get { return this._Components.AsReadOnly(); }
		}

		/// <summary>
		/// Lista atrybutów.
		/// </summary>
		public ReadOnlyCollection<Attribute> Attributes
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
		/// Dodaje komponent do encji.
		/// </summary>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane gdy taki komponent został już dodany.</exception>
		/// <param name="component">Komponent. Musi być unikatowy.</param>
		public void AddComponent(Component component)
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
			component.Owner = this;
			component.Init();
		}

		/// <summary>
		/// Dodaje atrybut do encji.
		/// </summary>
		/// <exception cref="Exceptions.ArgumentAlreadyExistsException">Rzucane gdy taki atrybut został już dodany.</exception>
		/// <param name="attribute">Atrybut. Musi być unikatowy.</param>
		public void AddAttribute(Attribute attribute)
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
		public Attribute GetAttribute(string id)
		{
			return this._Attributes.Find((a) => a.Id == id);
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
