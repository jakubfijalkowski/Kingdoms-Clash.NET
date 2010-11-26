using OpenTK;

namespace ClashEngine.NET.Graphics.Objects
{
	using Interfaces.Graphics;
	using Interfaces.Graphics.Objects;
	using Utilities;

	/// <summary>
	/// Prostokąt.
	/// </summary>
	public class Quad
		: IQuad
	{
		#region Private fields
		protected Vertex[] _Vertices = new Vertex[4]
		{
			new Vertex { Color = Vector4.One },
			new Vertex { Color = Vector4.One },
			new Vertex { Color = Vector4.One },
			new Vertex { Color = Vector4.One }
		};
		private int[] _Indecies = new int[]
		{
			0, 2, 3,
			0, 1, 2
		};
		#endregion

		#region IQuad Members
		/// <summary>
		/// Pozycja prostokąta.
		/// </summary>
		public Vector2 Position
		{
			get { return this._Vertices[0].Position; }
			set
			{
				this.UpdatePositions(value, this.Size);
			}
		}

		/// <summary>
		/// Rozmiar prostokąta.
		/// </summary>
		public Vector2 Size
		{
			get
			{
				return new Vector2(
					this._Vertices[1].Position.X - this._Vertices[0].Position.X,
					this._Vertices[3].Position.Y - this._Vertices[0].Position.Y);
			}
			set
			{
				this.UpdatePositions(this.Position, value);
			}
		}

		/// <summary>
		/// Kolor prostokąta.
		/// </summary>
		public OpenTK.Vector4 Color
		{
			get
			{
				return this.Vertices[0].Color;
			}
			set
			{
				for (int i = 0; i < this.Vertices.Length; i++)
				{
					this.Vertices[i].Color = value;
				}
			}
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Nie posiada tekstury.
		/// </summary>
		public Interfaces.Graphics.Resources.ITexture Texture { get { return null; } }

		/// <summary>
		/// Głębokość, na której znajduje się obiekt.
		/// </summary>
		public float Depth { get; set; }

		/// <summary>
		/// Wierzchołki obiektu.
		/// </summary>
		public Vertex[] Vertices { get { return this._Vertices; } }

		/// <summary>
		/// Indeksy.
		/// </summary>
		public int[] Indecies { get { return this._Indecies; } }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="depth">Głębokość, na której znajduje się prostokąt.</param>
		public Quad(Vector2 position, Vector2 size, Vector4 color, float depth = 0f)
		{
			this.UpdatePositions(position, size);
			this.Color = color;
			this.Depth = depth;
		}

		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="depth">Głębokość, na której znajduje się prostokąt.</param>
		public Quad(Vector2 position, Vector2 size, System.Drawing.Color color, float depth = 0f)
			: this(position, size, color.ToVector4(), depth)
		{ }
		#endregion

		#region Private methods
		private void UpdatePositions(Vector2 pos, Vector2 size)
		{
			this.Vertices[0].Position = pos;
			this.Vertices[1].Position = pos;
			this.Vertices[1].Position.X += size.X;
			this.Vertices[2].Position = pos + size;
			this.Vertices[3].Position = pos;
			this.Vertices[3].Position.Y += size.Y;
		}
		#endregion
	}
}
