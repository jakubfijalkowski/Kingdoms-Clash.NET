using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Encja gry - kontener na komponenty i atrybuty.
	/// </summary>
	[DebuggerDisplay("Entity {Id} with {Components.Count} components and {Attributes.Count} attributes")]
	public class GameEntity
		: IGameEntity
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IComponentsCollection _Components;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
		public IComponentsCollection Components
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
			this._Components = new ComponentsCollection(this);
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
			foreach (IRenderableComponent c in this.Components.RenderableComponents)
			{
				c.Render();
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji encji(dodaniu do managera).
		/// </summary>
		public virtual void OnInit()
		{ }

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji encji(usunięcie z managera).
		/// </summary>
		public virtual void OnDeinit()
		{
			this.Components.Clear();
		}
		#endregion
	}
}
