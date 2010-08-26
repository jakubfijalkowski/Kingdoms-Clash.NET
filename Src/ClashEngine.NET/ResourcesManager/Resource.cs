using System.IO;

namespace ClashEngine.NET.ResourcesManager
{
	using Interfaces.ResourcesManager;

	/// <summary>
	/// Klasa bazowa dla zasobów.
	/// </summary>
	public abstract class Resource
		: IResource
	{
		#region Properties
		/// <summary>
		/// Identyfikator zasobu - nazwa pliku z zasobem.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Ścieżka do zasobu.
		/// Absolutna, chyba, że metoda Init zostanie przeładowana.
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		public IResourcesManager Manager { get; private set; }
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje zasób.
		/// Do użytku wewnętrznego(jakoś trzeba zainicjalizować identyfikator w klasach dziedziczących).
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="manager">Manager-rodzic danego zasobu.</param>
		public virtual void Init(string id, IResourcesManager manager)
		{
			this.Free();
			this.Id = id;
			this.Manager = manager;
			this.FileName = Path.GetFullPath(Path.Combine(this.Manager.ContentDirectory, this.Id));
		}

		/// <summary>
		/// Ładuje zasób.
		/// </summary>
		/// <returns>Stan załadowania zasobu.</returns>
		public abstract ResourceLoadingState Load();

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		public abstract void Free();
		#endregion
	}
}
