using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using OpenTK;

namespace ClashEngine.NET.Components
{
	using EntitiesManager;
	using Extensions;
	using Interfaces.Components;

	/// <summary>
	/// Dodaje do ciała obiektu prostokąt je okalający.
	/// Musi być dodany po dodaniu PhysicalObject.
	/// </summary>
	public class BoundingBox
		: Component, IBoundingBox
	{
		#region IBoundingBox Members
		/// <summary>
		/// Rozmiar prostokąta.
		/// </summary>
		public Vector2 Size { get; private set; }

		/// <summary>
		/// Masa prostokąta.
		/// </summary>
		public float Mass { get; private set; }

		/// <summary>
		/// Fixture.
		/// </summary>
		public Fixture Fixture { get; private set; }
		#endregion

		#region Component Members
		/// <summary>
		/// Dodaje bounding boxa do ciała obiektu.
		/// </summary>
		public override void OnInit()
		{
			var body = base.Owner.Attributes.Get<Body>("Body");
			if (body == null)
			{
				throw new Exceptions.NotFoundException("Body", "Component PhysicalObject not added", null);
			}
			//Vertices verts = new Vertices(4);
			//verts.Add(this.Position.ToXNA());
			//verts.Add(new Microsoft.Xna.Framework.Vector2(this.Position.X + this.Size.X, this.Position.Y));
			//verts.Add((this.Position + this.Size).ToXNA());
			//verts.Add(new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y + this.Size.Y));


			this.Fixture = FarseerPhysics.Factories.FixtureFactory.CreateRectangle(this.Size.X, this.Size.Y,
				this.Mass / (this.Size.X * this.Size.Y),
				new Microsoft.Xna.Framework.Vector2(this.Size.X / 2f, this.Size.Y / 2f), body.Value);

			//PolygonShape poly = new PolygonShape(verts, 0f);
			//this.Fixture = body.Value.CreateFixture(poly);
		}

		/// <summary>
		/// Niepotrzebujemy niczego uaktualniać.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{ }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy obiekt.
		/// </summary>
		/// <param name="size">Rozmiar prostokąta.</param>
		public BoundingBox(Vector2 size)
			: base("BoundingBox")
		{
			this.Size = size;
			this.Mass = size.X * size.Y;
		}

		/// <summary>
		/// Inicjalizuje nowy obiekt.
		/// </summary>
		/// <param name="size">Rozmiar prostokąta.</param>
		/// <param name="mass">Masa prostokąta.</param>
		public BoundingBox(Vector2 size, float mass)
			: base("BoundingBox")
		{
			this.Size = size;
			this.Mass = mass;
		}
		#endregion
	}
}
