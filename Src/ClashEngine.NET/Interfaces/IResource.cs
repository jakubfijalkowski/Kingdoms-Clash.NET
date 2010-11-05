namespace ClashEngine.NET.Interfaces
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
		: System.IDisposable
	{
		#region Properties
		/// <summary>
		/// Identyfikator zasobu.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Ścieżka do zasobu.
		/// Od implementacji zależy czy jest absolutna czy relatywna.
		/// </summary>
		string FileName { get; set; }

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		IResourcesManager Manager { get; set; }
		#endregion

		#region Methods
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
