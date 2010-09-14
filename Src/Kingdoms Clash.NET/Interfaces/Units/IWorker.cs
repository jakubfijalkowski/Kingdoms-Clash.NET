namespace Kingdoms_Clash.NET.Interfaces.Units
{
	using Resources;

	/// <summary>
	/// Pracownik.
	/// </summary>
	public interface IWorker
		: IUnit
	{
		/// <summary>
		/// Ładunek, który jednostka przenosi.
		/// Przy przypisywaniu należy sprawdzić czy rozmiar ładunku nie przekracza MaxCargoSize.
		/// </summary>
		IResource Cargo { get; set; }

		#region Statistics
		/// <summary>
		/// Maksymalny rozmiar ładunku.
		/// </summary>
		int MaxCargoSize { get; }
		#endregion
	}
}
