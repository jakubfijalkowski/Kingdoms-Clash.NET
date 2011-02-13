using System;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using OpenTK;

namespace ClashEngine.NET.Graphics.Components
{
	using EntitiesManager;
	using Interfaces.Graphics.Components;
	using Extensions;
	using FarseerPhysics.Factories;

	/// <summary>
	/// Komponent-teren 2D.
	/// Buduje teren za pomocą wierzchołków wierzchniej warstwy(to co jest "pod" jest dedukowane automatycznie) i wysokości terenu jako trójkątu.
	/// Wysokość jest wysokością od najniżej położonego wierzchołka.
	/// 
	/// Jeśli do encji został wcześniej dodany komponent PhysicalObject automatycznie dodaje do niego odpowiednie dane.
	/// </summary>
	public class Terrain
		: RenderableComponent, ITerrain
	{
		#region Private fields
		private Objects.Terrain _Terrain = null;
		private Vector2[] Vertices = null;
		#endregion

		#region ITerrain Members
		/// <summary>
		/// Wysokość terenu.
		/// </summary>
		public float Height { get; private set; }
		#endregion

		#region Component Members
		public override void OnInit()
		{
			this._Terrain = new Objects.Terrain(this.Height, this.Vertices);
			this.AddShapes();
		}

		public override void Render()
		{
			this.Renderer.Draw(this._Terrain);
		}

		public override void Update(double delta)
		{ }
		#endregion

		#region Constructors
		/// <summary>
		/// Tworzy komponent.
		/// </summary>
		/// <param name="height">Wysokość terenu.</param>
		/// <param name="terrain">Wierzchołki.</param>
		/// <exception cref="ArgumentException">Height jest mniejsze bądź równe 0.</exception>
		/// <exception cref="ArgumentNullException">Nie podano żadnego wierzchołka.</exception>
		public Terrain(float height, params Vector2[] terrain)
			: base("Terrain")
		{
			if (height <= 0.0)
			{
				throw new ArgumentException("Height must be greater than zero", "height");
			}
			else if (terrain == null || terrain.Length == 0)
			{
				throw new ArgumentNullException("terrain");
			}
			
			this.Height = height;
			this.Vertices = terrain;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Gdy mamy otworzone ciało obiektu fizycznego dodajemy do niego odpowiednie figury.
		/// </summary>
		private void AddShapes()
		{
			var bodyAttr = this.Owner.Attributes.Get<Body>("Body");
			bodyAttr.Value.UserData = this;
			if (bodyAttr != null)
			{
				for (int i = 0; i < this.Vertices.Length - 1; i++)
				{
					var f = FixtureFactory.CreateEdge(this.Vertices[i].ToXNA(), this.Vertices[i + 1].ToXNA(), bodyAttr.Value);
					f.Friction = 0.5f;
					f.CollisionFilter.CollisionCategories = Category.All;
				}
			}
		}
		#endregion
	}
}
