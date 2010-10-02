using System;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.Components;
using OpenTK;
using Cfg = Kingdoms_Clash.NET.Configuration;

namespace Kingdoms_Clash.NET.Maps
{
	using Interfaces.Map;

	public class DefaultMap
		: GameEntity, IMap
	{
		#region IMap Members
		public string Name
		{
			get { return "Default map"; }
		}

		public float Width
		{
			get { return 2f; }
		}

		public float Height
		{
			get { return 0.5f; }
		}

		public Interfaces.Resources.IResource CheckForResource(float beginig, float end, out float position)
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
		}
		#endregion

		public DefaultMap()
			: base("Map.DefaultMap")
		{ }

		public override void OnInit()
		{
			float margin = Cfg.Instance.MapMargin;
			float maxH = Cfg.Instance.ScreenSize.Y - margin - 20f;
			TerrainVertex[] vertices = new TerrainVertex[]
			{
				new TerrainVertex { Position = new Vector2(0f, margin + 0f), Color = new Vector4(0.2f, 1f, 0.2f, 1f)},
				new TerrainVertex { Position = new Vector2(20f, margin + 0f), Color = new Vector4(0.2f, 1f, 0.2f, 1f)},
				new TerrainVertex { Position = new Vector2(100f, margin + maxH), Color = new Vector4(0.2f, 1f, 0.2f, 1f)},
				new TerrainVertex { Position = new Vector2(180f, margin + 0f), Color = new Vector4(0.2f, 1f, 0.2f, 1f)},
				new TerrainVertex { Position = new Vector2(200f, margin + 0f), Color = new Vector4(0.2f, 1f, 0.2f, 1f)},
			};
			this.Components.Add(new Terrain((this.Height * Cfg.Instance.ScreenSize.Y) - maxH));
		}
	}
}
