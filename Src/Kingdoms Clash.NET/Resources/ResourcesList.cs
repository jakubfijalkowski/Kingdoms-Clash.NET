using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Resources
{
	using Interfaces.Resources;
	using System.Diagnostics;

	/// <summary>
	/// Stała lista zasobów dla gry.
	/// </summary>
	public class ResourcesList
		: IResourcesList
	{
		#region Singleton
		private static ResourcesList _Instance = null;

		/// <summary>
		/// Instancja listy.
		/// </summary>
		public static ResourcesList Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new ResourcesList();
				}
				return _Instance;
			}
		}
		#endregion

		#region Private fields
		private List<IResourceDescription> Descriptions = new List<IResourceDescription>();
		#endregion

		#region IResourcesList Members
		/// <summary>
		/// Pobiera opis zasobu dla wskazanego Id.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <returns>Opis bądź null, gdy taki zasób nie istnieje.</returns>
		public IResourceDescription this[string id]
		{
			get { return this.Descriptions.Find(rd => rd.Id == id); }
		}

		/// <summary>
		/// Sprawdza, czy istnieje taki zasób.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <returns>Czy istnieje.</returns>
		public bool Exists(string id)
		{
			return this.Descriptions.Find(rd => rd.Id == id) != null;
		}
		#endregion

		#region IEnumerable<IResourceDescription> Members
		public IEnumerator<IResourceDescription> GetEnumerator()
		{
			return this.Descriptions.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Descriptions.GetEnumerator();
		}
		#endregion

		#region Constructors
		private ResourcesList()
		{ }
		#endregion

		#region Internals
		internal void Init(List<IResourceDescription> resources)
		{
			Debug.Assert(this.Descriptions.Count == 0, "Resources already loaded");
			this.Descriptions.AddRange(resources);
		}
		#endregion
	}
}
