namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	using Player;
	using Units;

	/// <summary>
	/// Bazowy interfejs dla kontrolera gry.
	/// On odpowiada za administracje jednostkami, sprawdzanie zasobów itp.
	/// </summary>
	public interface IGameController
	{
		#region Properties
		/// <summary>
		/// Stan gry. Ustawiany przez IGameState.
		/// </summary>
		IGameState GameState { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Prosi o nową jednostkę i jeśli wszystkie wymagania są spełnione to dodaje ją do listy jednostek gracza i jako encje do gry.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <param name="player">Gracz, który o nią prosi.</param>
		/// <returns>Czy dodano nową jednostkę.</returns>
		bool RequestNewUnit(string id, IPlayer player);

		/// <summary>
		/// Wywoływane co aktualizacje.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

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
		/// Resetuje stan gry do wartości początkowych.
		/// </summary>
		void Reset();

		/// <summary>
		/// Wywoływane przy rozpoczęciu gry, pozwala ustawić początkowe wartości.
		/// </summary>
		void OnGameStarted();
		#endregion
	}
}
