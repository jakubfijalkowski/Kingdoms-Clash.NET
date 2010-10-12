using ClashEngine.NET.Interfaces.EntitiesManager;
using OpenTK;

namespace Kingdoms_Clash.NET.Interfaces.Map
{
	/// <summary>
	/// Identyfikuje zasób, który znajduje się na mapie w danym miejscu.
	/// </summary>
	public class ResourceOnMap
	{
		/// <summary>
		/// Identyfikator zasobu.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Wartość.
		/// </summary>
		public uint Value { get; set; }
	}

	/// <summary>
	/// Handler zdarzenia zderzenia się jednostki z zasobem.
	/// </summary>
	/// <param name="unit">Jednostka, która się zderza.</param>
	/// <param name="resource">Zasób, z którym się zderzyła.</param>
	/// <returns>Czy zebrano zasób.</returns>
	public delegate bool CollisionWithResourceEventHandler(Units.IUnit unit, ResourceOnMap resource);

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
		/// Rozmiar mapy.
		/// Zawsze stały.
		/// </summary>
		Vector2 Size { get; }

		/// <summary>
		/// Pozycja pierwszego zamku. Uwzględnia wszystkie przesunięcia(włącznie z rozmiarem zamku).
		/// Zawsze wskazuje na lewy górny róg.
		/// </summary>
		Vector2 FirstCastle { get; }

		/// <summary>
		/// Pozycja drugiego zamku. Uwzględnia wszystkie przesunięcia(włącznie z rozmiarem zamku).
		/// Zawsze wskazuje na lewy górny róg.
		/// </summary>
		Vector2 SecondCastle { get; }

		/// <summary>
		/// Zderzenie się jednostki z zasobem.
		/// </summary>
		event CollisionWithResourceEventHandler Collide;

		/// <summary>
		/// Resetuje stan gry(zaczyna ją od nowa).
		/// Po resecie mapa powinna być tak samo dziewicza jak przed.
		/// </summary>
		void Reset();
	}
}
