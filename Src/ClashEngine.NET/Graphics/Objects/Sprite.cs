using OpenTK;

namespace ClashEngine.NET.Graphics.Objects
{
	using Interfaces.Graphics.Objects;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Obiekt renderera - duszek.
	/// </summary>
	/// <remarks>
	/// Dzięki przysłonięciu właściwości Texture możemy bez przeszkód wykorzystać Quad jako bazę dla duszka.
	/// </remarks>
	public class Sprite
		: Quad, ISprite
	{
		#region Private fields
		private ITexture _Texture = null;
		private SpriteEffect _Effect = SpriteEffect.No;
		private bool _MaintainAspectRation = false;
		#endregion

		#region ISprite Members
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

		/// <summary>
		/// Wymusza zachowanie proporcji duszka.
		/// </summary>
		public bool MaintainAspectRatio
		{
			get { return this._MaintainAspectRation; }
			set
			{
				this._MaintainAspectRation = true;
			}
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Tekstura obiektu.
		/// </summary>
		public new ITexture Texture
		{
			get { return this._Texture; }
			private set
			{
				this._Texture = value;
				this.UpdateTexCoords();
			}
		}

		/// <summary>
		/// Rozmiar duszka.
		/// </summary>
		public new Vector2 Size
		{
			get { return base.Size; }
			set { base.Size = (this._MaintainAspectRation ? value * System.Math.Min(value.X / this.Texture.Width, value.Y / this.Texture.Height) : value); }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="texture">Tekstura.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="effect">Efekt.</param>
		public Sprite(ITexture texture, Vector2 position)
			: base(position, new Vector2(texture.Width, texture.Height), Vector4.One)
		{
			this.Texture = texture;
			this.Depth = 0f;
		}

		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="texture">Tekstura.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="effect">Efekt.</param>
		public Sprite(ITexture texture, Vector2 position, Vector2 size)
			: base(position, size, Vector4.One)
		{
			this.Texture = texture;
		}
		#endregion

		#region Private methods
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
			this.Vertices[1].TexCoord = new Vector2(right, top);
			this.Vertices[2].TexCoord = new Vector2(right, bottom);
			this.Vertices[3].TexCoord = new Vector2(left, bottom);
		}
		#endregion
	}
}
