namespace ClashEngine.NET.Interfaces.ResourcesManager
{
	/// <summary>
	/// Interfejs bazowy dla zasobu.
	/// </summary>
	public interface IResource
	{
		#region Properties
		/// <summary>
		/// Identyfikator zasobu - nazwa pliku z zasobem.
		/// </summary>
		string Id { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje zasób.
		/// MUSI ustawiać właściwość Id na tą wskazywaną przez parametr.
		/// Jeśli wywoływana jest więcej niż jeden raz zasób przed tym powinien zostać zwolniony.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		void Init(string id);

		/// <summary>
		/// Ładuje zasób.
		/// </summary>
		void Load();

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		void Free();
		#endregion
	}
}
