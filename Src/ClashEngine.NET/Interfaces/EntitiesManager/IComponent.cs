namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla komponentów.
	/// </summary>
	public interface IComponent
	{
		/// <summary>
		/// Identyfikator(nazwa) komponentu.
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Właściciel komponentu.
		/// </summary>
		IGameEntity Owner { get; }

		/// <summary>
		/// Wywoływane przy inicjalizacji komponentu w GameEntity.
		/// </summary>
		/// <param name="owner">Właściciel komponentu.</param>
		void Init(IGameEntity owner);

		/// <summary>
		/// Wywoływane przy uaktualnieniu.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		void Update(double delta);

		#region Events
		/// <summary>
		/// Zdarzenie wywoływane przy inicjalizacji komponentu(dodaniu do managera).
		/// </summary>
		void OnInit();

		/// <summary>
		/// Zdarzenie wywoływane przy deinicjalizacji komponentu(usunięcie z managera).
		/// </summary>
		void OnDeinit();
		#endregion
	}
}
