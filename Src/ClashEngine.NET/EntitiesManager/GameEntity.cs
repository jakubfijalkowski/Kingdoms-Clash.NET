using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces;
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Encja gry - kontener na komponenty i atrybuty.
	/// </summary>
	[DebuggerDisplay("Entity {Id} with {Components.Count} components and {Attributes.Count} attributes")]
	public class GameEntity
		: IGameEntity
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IComponentsCollection _Components;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IAttributesCollection _Attributes;
		#endregion

		#region Properties
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Manager encji, do którego encja należy.
		/// Ustawiany przez niego samego przy dodaniu encji.
		/// </summary>
		public IEntitiesManager OwnerManager { get; set; }

		/// <summary>
		/// Wejście.
		/// Ustawiane przez manager.
		/// </summary>
		public IInput Input { get; set; }

		/// <summary>
		/// Manager zasobów.
		/// Ustawiany przez właściciela, ale nie ma wymogu, by to właśnie jego używać.
		/// </summary>
		public Interfaces.IResourcesManager Content { get; set; }

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
