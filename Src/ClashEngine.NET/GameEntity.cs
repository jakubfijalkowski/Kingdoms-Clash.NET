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

		#region Properties
		/// <summary>
		/// Identyfikator encji.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Lista komponentów.
		/// </summary>
		public ReadOnlyCollection<Component> Components
		{
			get { return this._Components.AsReadOnly(); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje nową encję.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public GameEntity(string id)
		{
			this.Id = id;
		}

		/// <summary>
		/// Dodaje komponent do encji.
		/// </summary>
		/// <param name="component">Komponent.</param>
		public void AddComponent(Component component)
		{
			this._Components.Add(component);
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
		#endregion
	}
}
