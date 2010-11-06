using ClashEngine.NET.Interfaces.Graphics.Resources;
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
		private SpriteEffect _Effect = SpriteEffect.No;
		#endregion

		#region ISprite Members
		/// <summary>
		/// Pozycja.
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
		/// Rozmiar.
		/// </summary>
		public Vector2 Size
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

		/// <summary>
		/// Efekty.
		/// </summary>
		public SpriteEffect Effect
		{
			get { return this._Effect; }
			set
			{
				this._Effect = value;
				this.UpdateTexCoords();
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
		public float Depth { get; set; }

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
		/// <param name="effect">Efekt.</param>
		public Sprite(ITexture texture, Vector2 position)
		{
			this.Texture = texture;
			this.Depth = 0f;
			this.UpdatePositions(position, new Vector2(texture.Width, texture.Height));
		}

		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="texture">Tekstura.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="effect">Efekt.</param>
		public Sprite(ITexture texture, Vector2 position, Vector2 size)
		{
			this.Texture = texture;
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
			float left, right;
			float top, bottom;

			if ((this.Effect & SpriteEffect.FlipHorizontally) == SpriteEffect.FlipHorizontally)
			{
				left = this.Texture.Coordinates.Right;
				right = this.Texture.Coordinates.Left;
			}
			else
			{
				right = this.Texture.Coordinates.Right;
				left = this.Texture.Coordinates.Left;
			}

			if ((this.Effect & SpriteEffect.FlipVertically) == SpriteEffect.FlipVertically)
			{
				top = this.Texture.Coordinates.Bottom;
				bottom = this.Texture.Coordinates.Top;
			}
			else
			{
				bottom = this.Texture.Coordinates.Bottom;
				top = this.Texture.Coordinates.Top;
			}

			this.Vertices[0].TexCoord = new Vector2(left, top);
			this.Vertices[1].TexCoord = new Vector2(right, bottom);
			this.Vertices[2].TexCoord = new Vector2(left, bottom);
			this.Vertices[3].TexCoord = new Vector2(left, top);
			this.Vertices[4].TexCoord = new Vector2(right, top);
			this.Vertices[5].TexCoord = new Vector2(right, bottom);
		}
		#endregion
	}
}
