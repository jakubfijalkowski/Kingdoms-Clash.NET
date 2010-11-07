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

		public float GetHeight(float x)
		{
			//Wybieramy kawałek na którym jest x
			Vector2 v1, v2;
			this.GetVerticesFor(x, out v1, out v2);

			//Wyznaczamy współczynnik i wyraz wolny
			float a, b;
			this.CalculateFactor(v1, v2, out a, out b);
			return a * x + b;
		}

		public float GetHeight(float x1, float x2)
		{
			Vector2 v11, v12, v21, v22;
			this.GetVerticesFor(x1, out v11, out v12);
			this.GetVerticesFor(x2, out v21, out v22);

			float a1, b1, a2, b2;
			this.CalculateFactor(v11, v12, out a1, out b1);
			this.CalculateFactor(v21, v22, out a2, out b2);
			if (v11 != v21 && a1 < 0f) //Pierwsza część rośnie, więc wybieramy wysokość czubka
			{
				return v12.Y;
			}
			return Math.Min(a1 * x1 + b1, a2 * x2 + b2);
		}
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

		#region Private methods
		private void GetVerticesFor(float x, out Vector2 v1, out Vector2 v2)
		{
			v1 = v2 = Vector2.Zero;
			for (int i = 0; i < this.Vertices.Length - 1; ++i)
			{
				if (this.Vertices[i].X <= x && this.Vertices[i + 1].X >= x)
				{
					v1 = this.Vertices[i];
					v2 = this.Vertices[i + 1];
					break;
				}
			}
		}

		private void CalculateFactor(Vector2 v1, Vector2 v2, out float a, out float b)
		{
			a = (v2.Y - v1.Y) / (v2.X - v1.X);
			b = -(a * v1.X - v1.Y);
		}
		#endregion
	}
}
