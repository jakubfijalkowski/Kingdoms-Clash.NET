using System;
using System.Collections.Generic;

namespace ClashEngine.NET.ResourcesManager
{
	using ClashEngine.NET.Interfaces.ResourcesManager;

	/// <summary>
	/// Manager zasobów.
	/// Zaimplementowany jako singleton - instancja pobierana przez właściwość Instance.
	/// </summary>
	public class ResourcesManager
		: IResourcesManager
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

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

		private Dictionary<string, IResource> Resources = new Dictionary<string, IResource>();

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
			where T : class, IResource, new()
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentNullException("filename");
			}
			IResource res;
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
			this.LoadResource(filename, newRes);
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
		public IResource Load(string filename, IResource res)
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentNullException("filename");
			}
			else if (res == null)
			{
				throw new ArgumentNullException("res");
			}
			IResource res1;
			if (this.Resources.TryGetValue(filename, out res1))
			{
				return res1;
			}

			this.LoadResource(filename, res);
			return res;
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		/// <exception cref="ArgumentNullException">Rzucane gdy res jest równe null.</exception>
		/// <exception cref="Exceptions.NotFoundException">Rzucane gdy nie znaleziono zasobu w managerze.</exception>
		/// <param name="res">Zasób.</param>
		public void Free(IResource res)
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
			Logger.Info("Resource '{0}' freed", res.Id);
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
			IResource res;
			if (!this.Resources.TryGetValue(id, out res))
			{
				throw new Exceptions.NotFoundException("Resource with id" + id);
			}
			this.Resources.Remove(id);
			res.Free();
			Logger.Info("Resource '{0}' freed", res.Id);
		}
		#endregion

		#region Others
		/// <summary>
		/// Uogólnia ładowanie zasobu - logowanie w jedynm miejscu.
		/// </summary>
		/// <param name="res"></param>
		private void LoadResource(string id, IResource res)
		{
			res.Init(id);
			switch (res.Load())
			{
			case ResourceLoadingState.Success:
				this.Resources.Add(id, res);
				Logger.Info("Resource '{0}' of type '{1}' loaded succesfully.", id, res.GetType().ToString());
				break;

			case ResourceLoadingState.Failure:
				Logger.Error("Cannot load resource '{0}' of type '{1}'", id, res.GetType().ToString());
				break;

			case ResourceLoadingState.DefaultUsed:
				this.Resources.Add(id, res);
				Logger.Warn("Cannot load resource '{0}' of type '{1}'. Default used.", id, res.GetType().ToString());
				break;
			}
		}

		#region IDisposable members
		public void Dispose()
		{
			foreach (var res in this.Resources)
			{
				res.Value.Free();
				Logger.Info("Resource {0} freed", res.Key);
			}
			this.Resources.Clear();
		}
		#endregion
		#endregion
	}
}
