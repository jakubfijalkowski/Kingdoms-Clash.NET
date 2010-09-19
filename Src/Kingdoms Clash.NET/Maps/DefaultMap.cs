using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.Components;
using OpenTK;

namespace Kingdoms_Clash.NET.Maps
{
	using Interfaces.Map;

	/// <summary>
	/// Domyślna mapa.
	/// </summary>
	public class DefaultMap
		: GameEntity, IMap
	{
		ITerrain Terrain;

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

		public override void InitEntity()
		{
			this.Terrain = new Terrain(0.5f,
				new TerrainVertex { Position = new Vector2(0.0f, 0.0f), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(0.1f, 0.0f), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(0.5f, -0.5f), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(0.9f, 0.0f), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(1.0f, 0.0f), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) });

			this.AddComponent(this.Terrain);
		}
	}
}
