using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Interfaces.Map
{
	using Resources;

	/// <summary>
	/// Mapa.
	/// Implementacja powinna zadbać o odnawianie się zasobów(jeśli są odnawialne).
	/// </summary>
	public interface IMap
		: IGameEntity
	{
		/// <summary>
		/// Nazwa mapy.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Szerokość mapy w metrach.
		/// Musi być stały dla danej mapy.
		float Width { get; }

		/// <summary>
		/// Wysokość mapy(samej mapy, bez marginesu górnego!) w metrach.
		/// </summary>
		float Height { get; }

		/// <summary>
		/// Wysokość, na której znajdują się zamki.
		/// </summary>
		float CastlePlacePosition { get; }

		/// <summary>
		/// Sprawdza czy pomiędzy dwoma punktami jest jakiś zasób.
		/// </summary>
		/// <param name="beginig">Początek.</param>
		/// <param name="end">Koniec szukania.</param>
		/// <param name="position">Pozycja na której odnaleziono zasób.</param>
		/// <returns>Pakunek z zasobem lub null, gdy nie znaleziono.</returns>
		IResource CheckForResource(float beginig, float end, out float position);

		/// <summary>
		/// Resetuje stan gry(zaczyna ją od nowa).
		/// Po resecie mapa powinna być tak samo dziewicza jak przed.
		/// </summary>
		void Reset();
	}
}
