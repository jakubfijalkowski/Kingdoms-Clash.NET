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

		public float Width
		{
			get { return 200f; }
		}

		public float Height
		{
			get { return 37.5f; }
		}

		public float CastlePlacePosition { get; private set; }

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
			float margin = this.CastlePlacePosition = Math.Max(Cfg.Instance.MapMargin, Cfg.Instance.ScreenSize.Y - this.Height); //Margines, wyrównujemy mapę tak, by sięgała dołu ekranu ale nie była mniejsza niż margines
			float maxH = 20f;
			TerrainVertex[] vertices = new TerrainVertex[]
			{
				new TerrainVertex
				{
					Position = new Vector2(0f, margin + 0f),
					Color = new Vector4(0.2f, 1f, 0.2f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2(Cfg.Instance.CastleSize.X, margin + 0f),
					Color = new Vector4(0.2f, 1f, 0.2f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2((200f - Cfg.Instance.CastleSize.X - 20f) / 2 + 20f, margin + maxH),
					Color = new Vector4(0.2f, 1f, 0.2f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2(200f - Cfg.Instance.CastleSize.X, margin + 0f),
					Color = new Vector4(0.2f, 1f, 0.2f, 1f)
				},
				new TerrainVertex
				{
					Position = new Vector2(200f, margin + 0f),
					Color = new Vector4(0.2f, 1f, 0.2f, 1f)
				},
			};
			this.Components.Add(new ClashEngine.NET.PhysicsManager.PhysicalObject());
			this.Components.Add(new Terrain((this.Height * Cfg.Instance.ScreenSize.Y) - maxH, vertices));
		}
	}
}
