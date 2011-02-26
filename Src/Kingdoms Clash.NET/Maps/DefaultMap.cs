using System;
using System.Diagnostics;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Graphics.Components;
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
		#region Private Fields
		private Vector2[] Vertices = null;
		#endregion

		#region IMap Members
		public string Name
		{
			get { return "Default map"; }
		}

		public Vector2 Size { get { return new Vector2(200f, 37.8f); } }

		public Vector2 FirstCastle { get; private set; }

		public Vector2 SecondCastle { get; private set; }
		#endregion

		#region Constructors
		public DefaultMap()
			: base("Map.DefaultMap")
		{ }
		#endregion

		#region GameEntity Members
		public override void OnInit()
		{
			float margin = Math.Max(Cfg.Instance.MapMargin, Cfg.Instance.ScreenSize.Y - this.Size.Y); //Margines, wyrównujemy mapę tak, by sięgała dołu ekranu ale nie była mniejsza niż margines
			float maxH = 20f;

			this.FirstCastle = new Vector2(0f, margin - Cfg.Instance.CastleSize.Y);
			this.SecondCastle = new Vector2(this.Size.X - Cfg.Instance.CastleSize.X, margin - Cfg.Instance.CastleSize.Y);

			this.Vertices = new Vector2[]
			{
				new Vector2(0f, margin + 0f),
				new Vector2(Cfg.Instance.CastleSize.X, margin + 0f),
				new Vector2((200f - Cfg.Instance.CastleSize.X - 20f) / 2 + 20f, margin + maxH),
				new Vector2(200f - Cfg.Instance.CastleSize.X, margin + 0f),
				new Vector2(200f, margin + 0f)
			};
			this.Components.Add(new ClashEngine.NET.Components.PhysicalObject());
			this.Components.Add(new Terrain(this.Size.Y - maxH, Vertices));
		}
		#endregion
	}
}
