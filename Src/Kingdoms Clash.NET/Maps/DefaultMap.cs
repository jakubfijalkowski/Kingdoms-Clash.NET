using System;
using System.Diagnostics;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.Components;
using OpenTK;
using Cfg = Kingdoms_Clash.NET.Configuration;

namespace Kingdoms_Clash.NET.Maps
{
	using Interfaces.Map;

	/// <summary>
	/// Domyślna mapa.
	/// </summary>
	[DebuggerDisplay("Default map")]
	public class DefaultMap
		: GameEntity, IMap
	{
		#region IMap Members
		public string Name
		{
			get { return "Default map"; }
		}

		public Vector2 Size { get { return new Vector2(200f, 37.8f); } }

		public Vector2 FirstCastle { get; private set; }

		public Vector2 SecondCastle { get; private set; }

		public int CheckForResource(float beginig, float end, out float position, out string id)
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
			float margin = Math.Max(Cfg.Instance.MapMargin, Cfg.Instance.ScreenSize.Y - this.Size.Y); //Margines, wyrównujemy mapę tak, by sięgała dołu ekranu ale nie była mniejsza niż margines
			float maxH = 20f;

			this.FirstCastle = new Vector2(0f, margin - Cfg.Instance.CastleSize.Y);
			this.SecondCastle = new Vector2(this.Size.X - Cfg.Instance.CastleSize.X, margin - Cfg.Instance.CastleSize.Y);

			TerrainVertex[] vertices = new TerrainVertex[]
			{
				new TerrainVertex
				{
					Position = new Vector2(0f, margin + 0f),
					Color = new Vector4(0f, 0.6f, 0f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2(Cfg.Instance.CastleSize.X, margin + 0f),
					Color = new Vector4(0f, 0.6f, 0f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2((200f - Cfg.Instance.CastleSize.X - 20f) / 2 + 20f, margin + maxH),
					Color = new Vector4(0f, 0.6f, 0f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2(200f - Cfg.Instance.CastleSize.X, margin + 0f),
					Color = new Vector4(0f, 0.6f, 0f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2(200f, margin + 0f),
					Color = new Vector4(0f, 0.6f, 0f, 1f)
				},
			};
			this.Components.Add(new ClashEngine.NET.PhysicsManager.PhysicalObject());
			this.Components.Add(new Terrain(this.Size.Y - maxH, vertices));
		}
	}
}
