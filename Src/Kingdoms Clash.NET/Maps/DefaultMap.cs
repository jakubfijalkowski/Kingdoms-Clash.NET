using System;
using System.Diagnostics;
using ClashEngine.NET.EntitiesManager;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using OpenTK;
using Cfg = Kingdoms_Clash.NET.Configuration;
using ClashEngine.NET.Extensions;

namespace Kingdoms_Clash.NET.Maps
{
	using Interfaces.Map;
	using ClashEngine.NET.Graphics.Components;

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
			//float margin = Math.Max(Settings.MapMargin, Cfg.Instance.ScreenSize.Y - this.Size.Y); //Margines, wyrównujemy mapę tak, by sięgała dołu ekranu ale nie była mniejsza niż margines
			float margin = Settings.MapMargin;
			float maxH = 20f;

			this.FirstCastle = new Vector2(0f, margin - Settings.CastleSize.Y);
			this.SecondCastle = new Vector2(this.Size.X - Settings.CastleSize.X, margin - Settings.CastleSize.Y);

			this.Vertices = new Vector2[]
			{
				new Vector2(0f, margin + 0f),
				new Vector2(Settings.CastleSize.X, margin + 0f),
				new Vector2((200f - Settings.CastleSize.X - 20f) / 2 + 20f, margin + maxH),
				new Vector2(200f - Settings.CastleSize.X, margin + 0f),
				new Vector2(200f, margin + 0f)
			};
			this.Components.Add(new ClashEngine.NET.Components.PhysicalObject());
			this.Attributes.Get<Body>("Body").Value.UserData = this;
			this.AddShapes();
#if !SERVER
			this.Components.Add(new ObjectHolder(new Internals.TerrainObject(this.Size.Y - maxH, this.Vertices)));
#endif
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Gdy mamy otworzone ciało obiektu fizycznego dodajemy do niego odpowiednie figury.
		/// </summary>
		private void AddShapes()
		{
			var bodyAttr = this.Attributes.Get<Body>("Body");
			bodyAttr.Value.UserData = this;
			if (bodyAttr != null)
			{
				for (int i = 0; i < this.Vertices.Length - 1; i++)
				{
					var f = FixtureFactory.CreateEdge(this.Vertices[i].ToXNA(), this.Vertices[i + 1].ToXNA(), bodyAttr.Value);
					f.Friction = 0.5f;
					f.CollisionFilter.CollisionCategories = Category.All;
					f.UserData = i;
				}
			}
		}
		#endregion
	}
}
