using System.Diagnostics;

namespace ClashEngine.NET.EntitiesManager
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Bazowa klasa dla komponentów.
	/// </summary>
	[DebuggerDisplay("Component {Id} of type {this.GetType().Name,nq}")]
	public abstract class Component
		: IComponent
	{
		/// <summary>
		/// Identyfikator(nazwa) komponentu.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Właściciel komponentu.
		/// </summary>
		public IGameEntity Owner { get; private set; }

		/// <summary>
		/// Inicjalizuje nowy komponent.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public Component(string id)
		{
			this.Id = id;
		}

		/// <summary>
		/// Wywoływane przy inicjalizacji komponentu w GameEntity. Służy np. do dodawania atrybutów.
		/// </summary>
		public void Init(IGameEntity owner)
		{
			this.Owner = owner;
		}

		/// <summary>
		/// Wywoływane przy uaktualnieniu.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		public abstract void Update(double delta);

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji komponentu(dodaniu do managera).
		/// </summary>
		public virtual void OnInit()
		{ }

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji komponentu(usunięcie z managera).
		/// </summary>
		public virtual void OnDeinit()
		{ }
		#endregion
	}
}
