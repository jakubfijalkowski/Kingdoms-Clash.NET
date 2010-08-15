using System;
using System.Collections.Generic;

namespace ClashEngine.NET.ResourcesManager
{
	/// <summary>
	/// Manager zasobów.
	/// Zaimplementowany jako singleton - instancja pobierana przez właściwość Instance.
	/// </summary>
	public class ResourcesManager
	{
		private Dictionary<string, Resource> Resources = new Dictionary<string, Resource>();

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
		/// Jeśli zasób był już załadowany i typ jest różny od rządanego rzuca wyjątkiem.
		/// </summary>
		/// <typeparam name="T">Rządany typ zasobu.</typeparam>
		/// <exception cref="InvalidCastException">Rzucane gdy rządany typ jest różny od typu załadowanego zasobu.</exception>
		/// <exception cref="ArgumentNullException">Rzucane gdy filename jest puste.</exception>
		/// <param name="filename">Nazwa pliku do załadowania.</param>
		/// <returns>Załadowany zasób.</returns>
		public T Load<T>(string filename)
			where T: Resource, new()
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentNullException("filename");
			}
			Resource res;
			if (this.Resources.TryGetValue(filename, out res)) //Znaleziono istniejący
			{
				if (!(res is T))
				{
					throw new InvalidCastException();
				}
				return res as T;
			}
			//Nie znaleziono
			T newRes = new T();
			newRes.Init(filename);
			newRes.Load();
			this.Resources.Add(filename, newRes);

			return newRes;
		}

		/// <summary>
		/// Ładuje zasób do już istniejącego.
		/// Jeśli zasób już był załadowany, zwraca go i obiekt podany w parametrze jest nieruszony, w przeciwnym razie zwraca parametr.
		/// Typ zwracanego zasobu NIE MUSI się zgadzać z typem parametru res.
		/// </summary>
		/// <param name="filename">Nazwa pliku.</param>
		/// <exception cref="ArgumentNullException">Rzucane gdy filename jest puste lub res jest równe null.</exception>
		/// <param name="res">Zasób.</param>
		public Resource Load(string filename, Resource res)
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentNullException("filename");
			}
			else if (res == null)
			{
				throw new ArgumentNullException("res");
			}
			Resource res1;
			if (this.Resources.TryGetValue(filename, out res1))
			{
				return res1;
			}
			res.Init(filename);
			res.Load();
			this.Resources.Add(filename, res);

			return res;
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <exception cref="ArgumentNullException">Rzucane gdy res jest równe null.</exception>
		/// <exception cref="Exceptions.NotFoundException">Rzucane gdy nie znaleziono zasobu w managerze.</exception>
		/// <param name="res">Zasób.</param>
		public void Free(Resource res)
		{
			if (res == null)
			{
				throw new ArgumentNullException("res");
			}
			if (!this.Resources.Remove(res.Id))
			{
				throw new Exceptions.NotFoundException("Resource with id" + res.Id);
			}
			res.Free();
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <exception cref="ArgumentNullException">Rzucane gdy id jest puste.</exception>
		/// <exception cref="Exceptions.NotFoundException">Rzucane gdy nie znaleziono zasobu w managerze.</exception>
		/// <param name="id">Identyfikator zasobu.</param>
		public void Free(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentNullException("id");
			}
			Resource res;
			if (!this.Resources.TryGetValue(id, out res))
			{
				throw new Exceptions.NotFoundException("Resource with id" + id);
			}
			this.Resources.Remove(id);
			res.Free();
		}
		#endregion
	}
}
