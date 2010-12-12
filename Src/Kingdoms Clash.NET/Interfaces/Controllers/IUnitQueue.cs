using System.ComponentModel;

namespace Kingdoms_Clash.NET.Interfaces.Controllers
{
	using Player;

	/// <summary>
	/// Kolejka produkcji jednostek.
	/// </summary>
	/// <remarks>
	/// Wywołuje PropertyChanged jeśli zmieni się długość kolejki.
	/// </remarks>
	public interface IUnitQueue
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Gracz, do któreo dana kolejka należy.
		/// </summary>
		IPlayer Player { get; }

		/// <summary>
		/// Całkowita długość kolejki.
		/// </summary>
		uint QueueLength { get; }

		/// <summary>
		/// Maksymalna liczba produkowanych jednostek jednego typu "na raz".
		/// </summary>
		/// <remarks>
		/// Nie może być większa niż <see cref="MaxProducedUnits"/>.
		/// </remarks>
		uint MaxProducedUnitsOfOneType { get; }

		/// <summary>
		/// Maksymalna liczba produkowanych jednostek "na raz".
		/// </summary>
		uint MaxProducedUnits { get; }

		/// <summary>
		/// Pobiera dłogość kolejki produkcyjnej danej jednostki.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <returns></returns>
		uint this[string id] { get; }
		
		/// <summary>
		/// Uaktualnia kolejkę.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		/// <returns>Jeśli jakaś jednostka została utworzona to zwraca jej instancje, w przeciwnym razie zwraca null.</returns>
		Units.IUnit Update(double delta);

		/// <summary>
		/// Prosi o dodanie jednostki do kolejki.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <returns>Token lub null, gdy nie dało się utworzyć jednostki.</returns>
		Units.IUnitRequestToken Request(string id);

		/// <summary>
		/// Czyści kolejkę.
		/// </summary>
		void Clear();
	}
}
