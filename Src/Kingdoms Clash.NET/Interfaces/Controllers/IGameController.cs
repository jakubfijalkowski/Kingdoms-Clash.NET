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
		#region Properties
		/// <summary>
		/// Gracze - zawsze dwóch.
		/// </summary>
		IPlayer[] Players { get; }

		/// <summary>
		/// Mapa.
		/// </summary>
		IMap Map { get; }

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
		/// Nie powinna zamieniać stanu gry - jej zadaniem jest np. sprawdzenie, czy jednostka może zebrać jakiś zasób, czy nie.
		/// </summary>
		/// <param name="delta">Czas od ostatniej aktualizacji.</param>
		void Update(double delta);

		/// <summary>
		/// Metoda-zdarzenie odpowiedzialna za kolizje gracza(jego zamku) z jednostką.
		/// </summary>
		/// <param name="player">Gracz(zamek gracza).</param>
		/// <param name="unit">Jednostka, która się z nim zderzyła.</param>
		void HandleCollision(IPlayer player, IUnit unit);

		/// <summary>
		/// Metoda-zdarzenie odpowiedzialna za kolizję jednostka-jednostka.
		/// Jednostki zawsze są przeciwnych graczy.
		/// </summary>
		/// <param name="unitA">Jednostka.</param>
		/// <param name="unitB">Jednostka.</param>
		void HandleCollision(IUnit unitA, IUnit unitB);
		#endregion
	}
}
