using ClashEngine.NET.Interfaces.Graphics.Resources;
using ClashEngine.NET.Utilities;
using OpenTK;

namespace ClashEngine.NET.Graphics.Objects
{
	using Interfaces.Graphics;
	using Interfaces.Graphics.Objects;

	/// <summary>
	/// Obiekt renderera - duszek.
	/// </summary>
	public class Sprite
		: ISprite
	{
		#region Private fields
		private Vertex[] _Vertices = new Vertex[6]
			{
				new Vertex { Color = Vector4.One },
				new Vertex { Color = Vector4.One },
				new Vertex { Color = Vector4.One },
				new Vertex { Color = Vector4.One },
				new Vertex { Color = Vector4.One },
				new Vertex { Color = Vector4.One }
			};
		private ITexture _Texture = null;
		#endregion

		#region ISprite Members
		/// <summary>
		/// Pozycja duszka.
		/// </summary>
		public OpenTK.Vector2 Position
		{
			get { return this._Vertices[0].Position; }
			set
			{
				this.UpdatePositions(value, this.Size);
			}
		}

		/// <summary>
		/// Rozmiar duszka.
		/// </summary>
		public OpenTK.Vector2 Size
		{
			get
			{
				return new Vector2(
					this._Vertices[4].Position.X - this._Vertices[0].Position.X,
					this._Vertices[2].Position.Y - this._Vertices[0].Position.Y);
			}
			set
			{
				this.UpdatePositions(this.Position, value);
			}
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Tekstura obiektu.
		/// </summary>
		public ITexture Texture
		{
			get { return this._Texture; }
			private set
			{
				this._Texture = value;
				this.UpdateTexCoords();
			}
		}

		/// <summary>
		/// Głębokość, na której znajduje się obiekt.
		/// </summary>
		public float Depth { get; private set; }

		/// <summary>
		/// Wierzchołki obiektu.
		/// </summary>
		public Vertex[] Vertices { get { return this._Vertices; } }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="texture">Tekstura.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="depth">Głębokość, na której się znajduje.</param>
		public Sprite(ITexture texture, Vector2 position, float depth = 0f)
		{
			this.Texture = texture;
			this.Depth = depth;
			this.UpdatePositions(position, new Vector2(texture.Width, texture.Height));
		}

		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="texture">Tekstura.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="depth">Głębokość, na której się znajduje.</param>
		public Sprite(ITexture texture, Vector2 position, Vector2 size, float depth = 0f)
		{
			this.Texture = texture;
			this.Depth = depth;
			this.UpdatePositions(position, size);
		}
		#endregion

		#region Private methods
		private void UpdatePositions(Vector2 pos, Vector2 size)
		{
			this.Vertices[0].Position = pos;
			this.Vertices[1].Position = pos + size;
			this.Vertices[2].Position = pos;
			this.Vertices[2].Position.Y += size.Y;

			this.Vertices[3].Position = pos;
			this.Vertices[4].Position = pos;
			this.Vertices[4].Position.X += size.X;
			this.Vertices[5].Position = pos + size;
		}

		private void UpdateTexCoords()
		{
			this.Vertices[0].TexCoord = this.Texture.Coordinates.TopLeft();
			this.Vertices[1].TexCoord = this.Texture.Coordinates.BottomRight();
			this.Vertices[2].TexCoord = this.Texture.Coordinates.BottomLeft();
			this.Vertices[3].TexCoord = this.Texture.Coordinates.TopLeft();
			this.Vertices[4].TexCoord = this.Texture.Coordinates.TopRight();
			this.Vertices[5].TexCoord = this.Texture.Coordinates.BottomRight();
		}
		#endregion
	}
}
