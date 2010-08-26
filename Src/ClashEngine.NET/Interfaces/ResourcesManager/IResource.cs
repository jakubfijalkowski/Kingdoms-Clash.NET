namespace ClashEngine.NET.Interfaces.ResourcesManager
{
	/// <summary>
	/// Stan załadowania zasobu.
	/// </summary>
	public enum ResourceLoadingState
	{
		/// <summary>
		/// Załadowano prawidłowo.
		/// </summary>
		Success,
		
		/// <summary>
		/// Nie można było załadować.
		/// </summary>
		Failure,
		
		/// <summary>
		/// Użyto domyślnego zasobu.
		/// </summary>
		DefaultUsed
	}

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

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		IResourcesManager Manager { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje zasób.
		/// MUSI ustawiać właściwość Id na tą wskazywaną przez parametr.
		/// Jeśli wywoływana jest więcej niż jeden raz zasób przed tym powinien zostać zwolniony.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="manager">Manager-rodzic danego zasobu.</param>
		void Init(string id, IResourcesManager manager);

		/// <summary>
		/// Ładuje zasób.
		/// </summary>
		/// <returns>Stan załadowania zasobu.</returns>
		ResourceLoadingState Load();

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		void Free();
		#endregion
	}
}
