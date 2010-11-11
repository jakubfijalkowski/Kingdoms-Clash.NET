using System;

namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Interfejs bazowy dla managera zasobów.
	/// </summary>
	public interface IResourcesManager
		: IDisposable
	{
		#region Properties
		/// <summary>
		/// Pobiera liczbę wszystkich zasobów dodanych do managera.
		/// </summary>
		int TotalCount { get; }

		/// <summary>
		/// Ścieżka do zasobów.
		/// Może być podawana jako ścieżka relatywna.
		/// </summary>
		string ContentDirectory { get; set; }
		#endregion

		#region Basic operations
		/// <summary>
		/// Ładuje zasób lub pobiera z już załadowanych.
		/// Jeśli zasób był już załadowany i typ jest różny od rządanego rzuca wyjątkiem.
		/// </summary>
		/// <typeparam name="T">Rządany typ zasobu.</typeparam>
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
		/// <param name="res">Zasób.</param>
		IResource Load(string filename, IResource res);

		/// <summary>
		/// Wymusza dodanie do managera już załadowanego zasobu.
		/// </summary>
		/// <param name="res">Zasób.</param>
		void Add(IResource res);

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <param name="res">Zasób.</param>
		void Free(IResource res);

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		void Free(string id);
		#endregion
	}
}
