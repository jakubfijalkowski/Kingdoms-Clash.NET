namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	using Map;
	using Player;
	using Units;

	/// <summary>
	/// Bazowy interfejs dla kontrolera gry.
	/// On odpowiada za administracje jednostkami, sprawdzanie zasobów itp.
	/// </summary>
	public interface IGameController
	{
		/// <summary>
		/// Stan gry. Ustawiany przez IGameState.
		/// </summary>
		IGameState GameState { get; set; }

		/// <summary>
		/// Kolejka produkcji jednostek dla pierwszego gracza.
		/// </summary>
		IUnitQueue Player1Queue { get; }

		/// <summary>
		/// Kolejka produkcji jednostek dla drugiego gracza.
		/// </summary>
		IUnitQueue Player2Queue { get; }

		/// <summary>
		/// Pobiera kolejkę jednostek dla wskazanego gracza.
		/// </summary>
		/// <param name="player">Gracz.</param>
		/// <returns></returns>
		IUnitQueue this[IPlayer player] { get; }

		/// <summary>
		/// Prosi o nowy zasób na mapie.
		/// Jeśli nie chcemy, by się pojawił - zwracamy null.
		/// </summary>
		/// <param name="id">Id zasobu.</param>
		/// <returns>Zasób.</returns>
		IResourceOnMap RequestNewResource(string id);

		/// <summary>
		/// Wywoływane co aktualizacje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Resetuje stan gry do wartości początkowych.
		/// </summary>
		void Reset();

		#region Events
		/// <summary>
		/// Metoda-zdarzenie odpowiedzialna za kolizje gracza(jego zamku) z jednostką.
		/// </summary>
		/// <param name="unit">Jednostka, która się z nim zderzyła.</param>
		/// <param name="player">Gracz(zamek gracza).</param>
		void HandleCollision(IUnit unit, IPlayer player);

		/// <summary>
		/// Metoda-zdarzenie odpowiedzialna za kolizję jednostka-jednostka.
		/// Jednostki zawsze są przeciwnych graczy.
		/// </summary>
		/// <param name="unitA">Jednostka.</param>
		/// <param name="unitB">Jednostka.</param>
		void HandleCollision(IUnit unitA, IUnit unitB);

		/// <summary>
		/// Metoda-zdarzenie odpowiedzialna za kolizje jednostki z zasobem na mapie.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		/// <param name="resource">Zasób.</param>
		/// <returns>Czy zasób został zebrany.</returns>
		bool HandleCollision(IUnit unit, IResourceOnMap resource);

		/// <summary>
		/// Wywoływane przy rozpoczęciu gry, pozwala ustawić początkowe wartości.
		/// </summary>
		void OnGameStarted();
		#endregion
	}
}
