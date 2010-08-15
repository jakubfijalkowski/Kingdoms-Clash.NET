using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClashEngine.NET.ResourcesManager
{
	/// <summary>
	/// Manager zasobów.
	/// Zaimplementowany jako singleton - instancja pobierana przez właściwość Instance.
	/// </summary>
	public class ResourcesManager
	{
		private Dictionary<string, Resource> Resources;

		#region Singleton implementation
		private static ResourcesManager _Instance = null;

		/// <summary>
		/// Instancja(jedyna na aplikację) managera.
		/// </summary>
		public static ResourcesManager Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new ResourcesManager();
				}
				return _Instance;
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Pobiera liczbę wszystkich zasobów dodanych do managera.
		/// </summary>
		public int TotalCount
		{
			get
			{
				return this.Resources.Count;
			}
		}
		#endregion

		#region Ctors
		private ResourcesManager()
		{ }
		#endregion

		#region Basic operations
		/// <summary>
		/// Ładuje zasób lub pobiera z już załadowanych.
		/// </summary>
		/// <typeparam name="T">Rządany typ zasobu.</typeparam>
		/// <param name="filename">Nazwa pliku do załadowania.</param>
		/// <returns>Załadowany zasób.</returns>
		public T Load<T>(string filename)
			where T: Resource, new()
		{
			return null;
		}

		/// <summary>
		/// Ładuje zasób do już istniejącego.
		/// Jeśli zasób już był załadowany, zwraca go i obiekt podany w parametrze jest nieruszony, w przeciwnym razie zwraca parametr.
		/// </summary>
		/// <param name="filename">Nazwa pliku.</param>
		/// <param name="res">Zasób.</param>
		public Resource Load(string filename, Resource res)
		{
			return null;
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <param name="res">Zasób.</param>
		public void Free(Resource res)
		{
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		public void Free(string id)
		{
		}
		#endregion
	}
}
