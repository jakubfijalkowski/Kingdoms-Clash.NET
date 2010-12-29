using System.Collections.Generic;

namespace ClashEngine.NET.Interfaces.EntitiesManager
{
	/// <summary>
	/// Bazowy interfejs dla managera encji.
	/// Musi ustawiać właściwości OwnerManager i GameInfo encji.
	/// </summary>
	public interface IEntitiesManager
		: ICollection<IGameEntity>
	{
		/// <summary>
		/// Uaktualnia eszystkie encje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Renderuje wszystkie renderowalne komponenty encji.
		/// </summary>
		void Render();
	}
}
