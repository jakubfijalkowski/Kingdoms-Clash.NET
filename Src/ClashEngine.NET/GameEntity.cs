using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET
{
	/// <summary>
	/// Encja gry - kontener na komponenty i atrybuty.
	/// </summary>
	public class GameEntity
	{
		private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
		private List<Component> _Components = new List<Component>();
		private List<Attribute> _Attributes = new List<Attribute>();

		#region Properties
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		public string Id { get; private set; }

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
		/// <exception cref="Exceptions.AlreadyExistsException">Rzucane gdy taki komponent został już dodany.</exception>
		/// <param name="component">Komponent. Musi być unikatowy.</param>
		public void AddComponent(Component component)
		{
			if (this._Components.Contains(component))
			{
				throw new Exceptions.AlreadyExistsException(string.Format("component '{0}'", component.Id));
			}
			this._Components.Add(component);
		}

		/// <summary>
		/// Dodaje atrybut do encji.
		/// </summary>
		/// <exception cref="Exceptions.AlreadyExistsException">Rzucane gdy taki atrybut został już dodany.</exception>
		/// <param name="attribute">Atrybut. Musi być unikatowy.</param>
		public void AddAttribute(Attribute attribute)
		{
			if (this._Attributes.Contains(attribute))
			{
				throw new Exceptions.AlreadyExistsException(string.Format("attribute '{0}'", attribute.Id));
			}
			this._Attributes.Add(attribute);
		}

		/// <summary>
		/// Uaktualnia wszystkie komponenty.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		public void Update(double delta)
		{
			foreach (Component c in this.Components)
			{
				c.Update(delta);
			}
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
		#endregion
	}
}
