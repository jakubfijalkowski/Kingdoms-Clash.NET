using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.Components;
using ClashEngine.NET.PhysicsManager;
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
		/// Domyślna mapa jest szeroka tylko na dwie jednostki.
		/// </summary>
		public float Width
		{
			get { return 2.0f; }
		}

		/// <summary>
		/// Wysokość mapy.
		/// </summary>
		public float Height
		{
			get { return 0.5f; }
		}

		public Interfaces.Resources.IResource CheckForResource(float beginig, float end, out float position)
		{
			throw new System.NotImplementedException();
		}

		public void Reset()
		{
			//throw new System.NotImplementedException();
		}
		#endregion

		public DefaultMap()
			: base("Map.Default_Map")
		{ }

		public override void InitEntity()
		{
			float margin = Configuration.Instance.MapMargin;
			this.Terrain = new Terrain(this.Height - 0.3f,
				new TerrainVertex { Position = new Vector2(0.0f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(0.1f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(1.0f, margin + 0.3f), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(1.9f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(2.0f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) });

			PhysicsManager.Instance.Terrain = this.Terrain;
			this.AddComponent(this.Terrain);
		}
	}
}
