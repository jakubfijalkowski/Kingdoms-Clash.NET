using System;
using OpenTK;

namespace ClashEngine.NET.Graphics.Objects
{
	using Interfaces.Graphics;
	using Interfaces.Graphics.Objects;

	/// <summary>
	/// Teren 2D.
	/// Budowany na podstawie wierzchołków warstwy górnej i wysokości mapy.
	/// </summary>
	public class Terrain
		: ITerrain
	{
		#region ITerrain Members
		/// <summary>
		/// Wysokość mapy.
		/// </summary>
		public float Height { get; private set; }
		#endregion

		#region IObject Members
		public Interfaces.Graphics.Resources.ITexture Texture { get { return null; } }
		public float Depth { get; set; }
		public Vertex[] Vertices { get; private set; }
		public int[] Indecies { get; private set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="height">Wysokość terenu od najniżej położonego wierzchołka.</param>
		/// <param name="terrain">Wierzchołki.</param>
		public Terrain(float height, params Vector2[] terrain)
		{
			if (height <= 0.0)
			{
				throw new ArgumentException("Height must be greater than zero", "height");
			}
			else if (terrain == null || terrain.Length == 0)
			{
				throw new ArgumentNullException("vertices");
			}

			this.Depth = 5f;
			this.Height = height;

			this.Vertices = new Vertex[terrain.Length * 2];
			this.Indecies = new int[(this.Vertices.Length - 1) * 6];

			//Budujemy tablicę wierzchołków
			float lowest = this.GetLowestVertex(terrain) + this.Height; //Położenie krawędzi dolnej.
			for (int i = 0, j = 0; j < this.Vertices.Length; ++i, j += 2)
			{
				this.Vertices[j].Position = terrain[i];
				this.Vertices[j + 1].Position = terrain[i];
				this.Vertices[j + 1].Position.Y = lowest;

				this.Vertices[j].Color = this.Vertices[j + 1].Color = new Vector4(0f, 0.6f, 0f, 1f);
			}

			//Budujemy tablicę indeksów
			//Dwa trójkąty - pierwszy i ostatni muszą być ustawione przeze mnie
			this.Indecies[0] = 0;
			this.Indecies[1] = 1;
			this.Indecies[2] = 2;

			this.Indecies[this.Indecies.Length - 3] = (int)(this.Vertices.Length - 3);
			this.Indecies[this.Indecies.Length - 2] = (int)(this.Vertices.Length - 2);
			this.Indecies[this.Indecies.Length - 1] = (int)(this.Vertices.Length - 1);

			for (int i = 1, j = 3; i < terrain.Length - 1; i += 1, j += 6)
			{
				this.Indecies[j + 0] = (i * 2);
				this.Indecies[j + 1] = (i * 2) - 1;
				this.Indecies[j + 2] = (i * 2) + 1;

				this.Indecies[j + 3] = (i * 2);
				this.Indecies[j + 4] = (i * 2) + 1;
				this.Indecies[j + 5] = (i * 2) + 2;
			}
		} 
		#endregion

		#region Private members
		/// <summary>
		/// Pobiera najniżej położony wierzchołek(czyli z największą współrzędną Y).
		/// </summary>
		/// <returns>Współrzędna Y tego wierzchołka.</returns>
		private float GetLowestVertex(Vector2[] terrain)
		{
			float lowest = float.MinValue;
			foreach (var vertex in terrain)
			{
				if (vertex.Y > lowest)
				{
					lowest = vertex.Y;
				}
			}
			return lowest;
		}
		#endregion
	}
}
