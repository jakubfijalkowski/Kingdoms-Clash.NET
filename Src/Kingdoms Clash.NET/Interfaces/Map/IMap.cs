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

		/// <summary>
		/// Pobiera wysokość danego punktu.
		/// </summary>
		/// <param name="x">Ponkt na osi.</param>
		/// <returns>Wysokość(z uwzględnieniem marginesu mapy).</returns>
		float GetHeight(float x);

		/// <summary>
		/// Pobiera wysokość obiektu znajdującego się pomiędzy x1 a x2.
		/// </summary>
		/// <param name="x1">Pierwsza współrzędna.</param>
		/// <param name="x2">Druga współrzędna.</param>
		/// <returns>Wysokość(z uwzględnieniem marginesu mapy).</returns>
		float GetHeight(float x1, float x2);
	}
}
