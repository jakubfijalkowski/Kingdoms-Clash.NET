﻿using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using OpenTK;

namespace ClashEngine.NET.Components.Physical
{
	using EntitiesManager;
	using Interfaces.Components.Physical;
	using Utilities;

	/// <summary>
	/// Dodaje do ciała obiektu prostokąt je okalający.
	/// Musi być dodany po dodaniu PhysicalObject.
	/// </summary>
	public class BoundingBox
		: Component, IBoundingBox
	{
		#region IBoundingBox Members
		/// <summary>
		/// Fixture.
		/// </summary>
		public Fixture Fixture { get; private set; }
		#endregion

		#region Properties
		/// <summary>
		/// Rozmiar prostokąta.
		/// </summary>
		public Vector2 Size { get; private set; }

		/// <summary>
		/// Pozycja prostokąta w ciele.
		/// </summary>
		public Vector2 Position { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowy obiekt.
		/// </summary>
		/// <param name="size">Rozmiar prostokąta.</param>
		public BoundingBox(Vector2 size)
			: this(size, new Vector2())
		{ }

		/// <summary>
		/// Inicjalizuje nowy obiekt.
		/// </summary>
		/// <param name="size">Rozmiar prostokąta.</param>
		/// <param name="position">Pozycja prostokąta w ciele.</param>
		public BoundingBox(Vector2 size, Vector2 position)
			: base("BoundingBox")
		{
			this.Size = size;
			this.Position = position;
		}
		#endregion

		public override void OnInit()
		{
			var body = base.Owner.Attributes.Get<Body>("Body");
			if (body == null)
			{
				throw new Exceptions.NotFoundException("Body", "Component PhysicalObject not added", null);
			}
			Vertices verts = new Vertices(4);
			verts.Add(this.Position.ToXNA());
			verts.Add(new Microsoft.Xna.Framework.Vector2(this.Position.X + this.Size.X, this.Position.Y));
			verts.Add((this.Position + this.Size).ToXNA());
			verts.Add(new Microsoft.Xna.Framework.Vector2(this.Position.X, this.Position.Y + this.Size.Y));

			PolygonShape poly = new PolygonShape(verts);
			this.Fixture = body.Value.CreateFixture(poly);
		}

		#region Unused
		public override void Update(double delta)
		{ }
		#endregion
	}
}