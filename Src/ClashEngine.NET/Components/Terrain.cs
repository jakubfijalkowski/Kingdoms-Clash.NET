using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace ClashEngine.NET.Components
{
	using EntitiesManager;
	using Interfaces.Components;
	using Utilities;

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
		/// <summary>
		/// VBO na teren.
		/// </summary>
		VBO TerrainVBO = null;

		#region ITerrain Members
		/// <summary>
		/// Wierzchołki terenu.
		/// Tylko do odczytu.
		/// </summary>
		public IList<TerrainVertex> Vertices { get; private set; }

		/// <summary>
		/// Wysokość mapy.
		/// </summary>
		public float Height { get; private set; }
		#endregion

		/// <summary>
		/// Tworzy teren.
		/// </summary>
		/// <param name="height">Wysokość terenu.</param>
		/// <param name="terrain">Wierzchołki.</param>
		/// <exception cref="ArgumentException">Height jest mniejsze bądź równe 0.</exception>
		/// <exception cref="ArgumentNullException">Nie podano żadnego wierzchołka.</exception>
		public Terrain(float height, params TerrainVertex[] terrain)
			: base("Terrain")
		{
			if(height <= 0.0)
			{
				throw new ArgumentException("Height must be greater than zero", "height");
			}
			else if (terrain == null || terrain.Length == 0)
			{
				throw new ArgumentNullException("vertices");
			}

			this.Height = height;
			this.Vertices = new List<TerrainVertex>(terrain);
		}

		public override void Init(Interfaces.EntitiesManager.IGameEntity owner)
		{
			base.Init(owner);

			this.AddShapes();

			Vertex2DPC[] vertices = new Vertex2DPC[this.Vertices.Count * 2];
			uint[] indecies = new uint[(this.Vertices.Count - 1) * 6];

			//Budujemy tablicę wierzchołków
			float lowest = this.GetLowestVertex() + this.Height; //Położenie krawędzi dolnej.
			for (int i = 0, j = 0; j < vertices.Length; ++i, j += 2)
			{
				vertices[j].Position = this.Vertices[i].Position;
				vertices[j + 1].Position = this.Vertices[i].Position;
				vertices[j + 1].Position.Y = lowest;

				vertices[j].Color = vertices[j + 1].Color = this.Vertices[i].Color;
			}

			//Budujemy tablicę indeksów
			//Dwa trójkąty - pierwszy i ostatni muszą być ustawione przeze mnie
			indecies[0] = 0;
			indecies[1] = 1;
			indecies[2] = 2;

			indecies[indecies.Length - 3] = (uint)(vertices.Length - 3);
			indecies[indecies.Length - 2] = (uint)(vertices.Length - 2);
			indecies[indecies.Length - 1] = (uint)(vertices.Length - 1);

			for (uint i = 1, j = 3; i < this.Vertices.Count - 1; i += 1, j += 6)
			{
				indecies[j + 0] = (i * 2);
				indecies[j + 1] = (i * 2) - 1;
				indecies[j + 2] = (i * 2) + 1;

				indecies[j + 3] = (i * 2);
				indecies[j + 4] = (i * 2) + 1;
				indecies[j + 5] = (i * 2) + 2;
			}

			this.TerrainVBO = new VBO(indecies, vertices);
		}

		#region Initialization
		/// <summary>
		/// Pobiera najniżej położony wierzchołek(czyli z największą współrzędną Y).
		/// </summary>
		/// <returns>Współrzędna Y tego wierzchołka.</returns>
		private float GetLowestVertex()
		{
			float lowest = float.MinValue;
			foreach (var vertex in this.Vertices)
			{
				if (vertex.Position.Y > lowest)
				{
					lowest = vertex.Position.Y;
				}
			}
			return lowest;
		}

		/// <summary>
		/// Gdy mamy otworzone ciało obiektu fizycznego dodajemy do niego odpowiednie figury.
		/// </summary>
		private void AddShapes()
		{
			var bodyAttr = this.Owner.Attributes.Get<Body>("Body");
			if (bodyAttr != null)
			{				
				for (int i = 0; i < this.Vertices.Count - 1; i++)
				{
					PolygonShape sector = new PolygonShape();
					sector.SetAsEdge(this.Vertices[i].Position.ToXNA(), this.Vertices[i + 1].Position.ToXNA());

					bodyAttr.Value.CreateFixture(sector).Friction = 1f;
				}
			}
		}
		#endregion

		public override void Render()
		{
			this.TerrainVBO.Draw();
		}

		public override void Update(double delta)
		{ }
	}
}
