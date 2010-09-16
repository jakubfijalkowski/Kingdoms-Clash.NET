using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla managera encji.
	/// </summary>
	public interface IEntitiesManager
	{
		#region Properties
		/// <summary>
		/// Lista encji.
		/// Zmieniać za pomocą odpowiednich metod, nie ręcznie!
		/// </summary>
		IList<IGameEntity> Entities { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Dodaje encję.
		/// Nie można dodawać dwóch IDENTYCZNYCH(ten sam obiekt) encji - wiele encji o tym samym ID jest dozwolone.
		/// </summary>
		/// <param name="entity">Encja do dodania.</param>
		void AddEntity(IGameEntity entity);

		/// <summary>
		/// Usuwa encję.
		/// </summary>
		/// <param name="entity">Encja do usunięcia.</param>
		void RemoveEntity(IGameEntity entity);

		/// <summary>
		/// Uaktualnia eszystkie encje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Renderuje wszystkie renderowalne komponenty encji.
		/// </summary>
		void Render();
		#endregion
	}
}
