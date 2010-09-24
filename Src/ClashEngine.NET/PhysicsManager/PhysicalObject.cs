using System.Drawing;
using System.Linq;
using ClashEngine.NET.Interfaces.Components;
using OpenTK;

namespace ClashEngine.NET.PhysicsManager
{
	using EntitiesManager;
	using Interfaces.EntitiesManager;
	using Interfaces.PhysicsManager;

	/// <summary>
	/// Komponent fizyczny.
	/// Po dodaniu do encji staje się ona zależna od wszystkich czynników zewnętrznych.
	/// </summary>
	public class PhysicalObject
		: Component, IPhysicalObject
	{
		#region Private fields
		private IAttribute<IVelocitiesCollection> Velocities_ = null;
		private IAttribute<Vector2> Position_ = null;
		private IAttribute<Vector2> Size_ = null;

		/// <summary>
		/// Suma lokalnych(per-obiekt) prędkości. Ułatwia obliczenia.
		/// </summary>
		internal Vector2 LocalVelocity = new Vector2(0.0f, 0.0f);
		#endregion

		#region IPhysics Members
		/// <summary>
		/// Lista z prędkościami od których jest zależny dany obiekt.
		/// </summary>
		public IVelocitiesCollection Velocities
		{
			get { return this.Velocities_.Value; }
		}

		/// <summary>
		/// Pozycja obiektu.
		/// </summary>
		public Vector2 Position
		{
			get { return this.Position_.Value; }
			set { this.Position_.Value = value; }
		}

		/// <summary>
		/// Rozmiar obiektu.
		/// </summary>
		public Vector2 Size
		{
			get { return this.Size_.Value; }
			set { this.Size_.Value = value; }
		}
		#endregion

		#region Component Members
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			this.Velocities_ = this.Owner.Attributes.GetOrCreate<IVelocitiesCollection>("Velocities");
			this.Velocities_.Value = new InternalVelocitiesCollection(this); //Tego nie mogło być, więc musimy utworzyć nowe.

			this.Position_ = this.Owner.Attributes.GetOrCreate<Vector2>("Position");
			this.Size_ = this.Owner.Attributes.GetOrCreate<Vector2>("Size");
		}

		public override void Update(double delta)
		{
			this.Position += (float)delta * (this.LocalVelocity + PhysicsManager.Instance.CalculateVelocities(null));

			var bbox = new RectangleF(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y);

			this.CheckCollisionWithTerrain(bbox);
		}
		#endregion

		#region IEquatable<IComponent> Members
		public bool Equals(IComponent other)
		{
			return (other is PhysicalObject) && other == this;
		}
		#endregion

		public PhysicalObject()
			: base("PhysicalObject")
		{ }

		#region Collistion detection
		private bool CheckCollisionWithTerrain(RectangleF bbox)
		{
			IPhysicsManager pm = PhysicsManager.Instance;

			//Sprawdzamy, czy nie koliduje z terenem i jeśli tak - przesuwamy ponad niego.
			if (pm.Terrain != null &&
				bbox.Left > pm.Terrain.Vertices[0].Position.X && bbox.Left < pm.Terrain.Vertices[pm.Terrain.Vertices.Count - 1].Position.X)
			{
				//Najpierw sprawdzamy, czy któryś z wierzchołków terenu jest pod jednostką(wyjątek - jest to zagłębienie terenu).
				for (int i = 1; i < pm.Terrain.Vertices.Count - 1; i++)
				{
					if (pm.Terrain.Vertices[i].Position.X > bbox.Left && pm.Terrain.Vertices[i].Position.X < bbox.Right &&
						pm.Terrain.Vertices[i - 1].Position.Y >= pm.Terrain.Vertices[i].Position.Y && pm.Terrain.Vertices[i + 1].Position.Y >= pm.Terrain.Vertices[i].Position.Y
						)
					{
						this.Position = new Vector2(this.Position.X, pm.Terrain.Vertices[i].Position.Y - this.Size.Y);
						return true;
					}
				}

				//Wyszukujemy pary wierzchołków pomiędzy którymi jest nasz obiekt.
				TerrainVertex v1 = null, v2 = null;
				for (int i = 0; i < pm.Terrain.Vertices.Count - 1; i++)
				{
					if (pm.Terrain.Vertices[i].Position.X <= bbox.Left && pm.Terrain.Vertices[i + 1].Position.X >= bbox.Left)
					{
						v1 = pm.Terrain.Vertices[i];
						v2 = pm.Terrain.Vertices[i + 1];
						break;
					}
				}

				//Sprawdzamy, czy obiekt jest pod czy nad prostą używając równania prostej y = ax + b
				//Wyznaczamy współczynnik kierunkowy prostej i wyraz wolny.
				float a = (v2.Position.Y - v1.Position.Y) / (v2.Position.X - v1.Position.X);
				float b = v1.Position.Y - a * v1.Position.X;

				float y1 = a * bbox.Left + b; //Lewy wierzchołek
				float y2 = a * bbox.Right + b; //Prawy wierzchołek

				if (a == 0.0) //Prosta równoległa do osi X.
				{
					if (bbox.Bottom > v1.Position.Y)
					{
						this.Position = new Vector2(this.Position.X, v1.Position.Y - this.Size.Y);
					}
				}
				else if (a > 0.0 && y1 < bbox.Bottom) //Współczynnik kierunkowy jest dodatni - prosta "maleje" w naszym układzie
				{
					this.Position = new Vector2(this.Position.X, y1 - this.Size.Y);
					return true;
				}
				else if (y2 < bbox.Bottom)
				{
					this.Position = new Vector2(this.Position.X, y2 - this.Size.Y);
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}
