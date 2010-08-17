using System;

namespace ClashEngine.NET.Interfaces.ResourcesManager
{
	/// <summary>
	/// Interfejs bazowy dla managera zasobów.
	/// </summary>
	public interface IResourcesManager
	{
		#region Properties
		/// <summary>
		/// Pobiera liczbę wszystkich zasobów dodanych do managera.
		/// </summary>
		int TotalCount
		{
			get;
		}
		#endregion

		#region Basic operations
		/// <summary>
		/// Ładuje zasób lub pobiera z już załadowanych.
		/// Jeśli zasób był już załadowany i typ jest różny od rządanego rzuca wyjątkiem.
		/// </summary>
		/// <typeparam name="T">Rządany typ zasobu.</typeparam>
		/// <exception cref="InvalidCastException">Rzucane gdy rządany typ jest różny od typu załadowanego zasobu.</exception>
		/// <exception cref="ArgumentNullException">Rzucane gdy filename jest puste.</exception>
		/// <param name="filename">Nazwa pliku do załadowania.</param>
		/// <returns>Załadowany zasób.</returns>
		T Load<T>(string filename)
			where T : class, IResource, new();

		/// <summary>
		/// Ładuje zasób do już istniejącego.
		/// Jeśli zasób już był załadowany, zwraca go i obiekt podany w parametrze jest nieruszony, w przeciwnym razie zwraca parametr.
		/// Typ zwracanego zasobu NIE MUSI się zgadzać z typem parametru res.
		/// </summary>
		/// <param name="filename">Nazwa pliku.</param>
		/// <exception cref="ArgumentNullException">Rzucane gdy filename jest puste lub res jest równe null.</exception>
		/// <param name="res">Zasób.</param>
		IResource Load(string filename, IResource res);

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <exception cref="ArgumentNullException">Rzucane gdy res jest równe null.</exception>
		/// <exception cref="Exceptions.NotFoundException">Rzucane gdy nie znaleziono zasobu w managerze.</exception>
		/// <param name="res">Zasób.</param>
		void Free(IResource res);

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <exception cref="ArgumentNullException">Rzucane gdy id jest puste.</exception>
		/// <exception cref="Exceptions.NotFoundException">Rzucane gdy nie znaleziono zasobu w managerze.</exception>
		/// <param name="id">Identyfikator zasobu.</param>
		void Free(string id);
		#endregion
	}
}
