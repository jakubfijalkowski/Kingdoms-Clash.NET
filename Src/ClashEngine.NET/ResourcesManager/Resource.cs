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
		#endregion

		#region Methods
		/// <summary>
		/// Inicjalizuje zasób.
		/// Do użytku wewnętrznego(jakoś trzeba zainicjalizować identyfikator w klasach dziedziczących).
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public virtual void Init(string id)
		{
			this.Free();
			this.Id = id;
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
