using ClashEngine.NET.Interfaces.EntitiesManager;
using OpenTK;

namespace Kingdoms_Clash.NET.Interfaces.Map
{
	/// <summary>
	/// Mapa.
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
		/// Resetuje stan gry(zaczyna ją od nowa).
		/// Po resecie mapa powinna być tak samo dziewicza jak przed.
		/// </summary>
		void Reset();
	}
}
