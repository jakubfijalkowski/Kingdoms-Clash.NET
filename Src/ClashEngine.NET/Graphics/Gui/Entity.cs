using System;

namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.EntitiesManager;
	using Interfaces.Graphics.Gui;

	public class Entity
		: Container, IEntity
	{
		#region IGameEntity Members
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Manager encji, do którego encja należy.
		/// Ustawiany przez niego samego przy dodaniu encji.
		/// </summary>
		public Interfaces.EntitiesManager.IEntitiesManager OwnerManager { get; set; }

		/// <summary>
		/// Manager zasobów.
		/// Ustawiany przez właściciela, ale nie ma wymogu, by to właśnie jego używać.
		/// </summary>
		public Interfaces.IResourcesManager Content { get; set; }

		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji encji(dodaniu do managera).
		/// Służy np. do dodania kontrolek.
		/// </summary>
		public virtual void OnInit()
		{ }

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji encji(usunięcie z managera).
		/// </summary>
		public virtual void OnDeinit()
		{ }

		/// <summary>
		/// Niewspierane.
		/// </summary>
		public Interfaces.EntitiesManager.IComponentsCollection Components
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Niewspierane.
		/// </summary>
		public Interfaces.EntitiesManager.IAttributesCollection Attributes
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Uaktualnia kontrolki.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void IGameEntity.Update(double delta)
		{
			base.Update(delta);
			this.CheckControls();
		}

		/// <summary>
		/// Rysuje GUI.
		/// </summary>
		void IGameEntity.Render()
		{
			base.Render();
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje encję.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public Entity(string id)
		{
			this.Id = id;
		}
		#endregion

		#region Others
		/// <summary>
		/// Metoda-zdarzenie służąca do sprawdzenia kontrolek.
		/// </summary>
		public virtual void CheckControls()
		{ }
		#endregion
	}
}
