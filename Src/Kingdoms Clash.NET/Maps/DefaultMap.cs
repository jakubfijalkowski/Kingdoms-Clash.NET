using ClashEngine.NET.EntitiesManager;

namespace Kingdoms_Clash.NET.Maps
{
	using Interfaces.Map;

	/// <summary>
	/// Domyślna mapa.
	/// </summary>
	public class DefaultMap
		: GameEntity, IMap
	{
		#region IMap Members
		/// <summary>
		/// Nazwa mapy.
		/// </summary>
		public string Name
		{
			get { return "Default map"; }
		}

		/// <summary>
		/// Rozmiar mapy.
		/// Domyślna mapa jest szeroka tylko na jeden ekran.
		/// </summary>
		public float Size
		{
			get { return 1.0f; }
		}

		public Interfaces.Resources.IResource CheckForResource(float beginig, float end, out float position)
		{
			position = 0.0f;
			return null;
		}

		public void Reset()
		{
		}
		#endregion

		public DefaultMap()
			: base("Map.Default_Map")
		{ }
	}
}
