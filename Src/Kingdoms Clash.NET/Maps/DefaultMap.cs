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
		/// Domyślna mapa jest szeroka tylko na dwie jednostki.
		/// </summary>
		public float Width
		{
			get { return 200f; }
		}

		/// <summary>
		/// Wysokość mapy.
		/// </summary>
		public float Height
		{
			get { return 37.5f; }
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
			float holeHeight = this.Height * 0.9f;
			this.Terrain = new Terrain(this.Height - holeHeight,
				new TerrainVertex { Position = new Vector2(0f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(20f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(100f, margin + holeHeight), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(180f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) },
				new TerrainVertex { Position = new Vector2(200f, margin), Color = new Vector4(0.0f, 0.6f, 0.0f, 1.0f) });

			this.Components.Add(new ClashEngine.NET.PhysicsManager.PhysicalObject());
			this.Components.Add(this.Terrain);
		}
	}
}
