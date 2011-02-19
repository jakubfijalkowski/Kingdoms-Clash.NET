using OpenTK;
using System.Text.RegularExpressions;

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
		private static Regex AnimationInfoRegex = new Regex(@"(\d+),(\d+)x(\d+),(\d+[\.]?\d*)", RegexOptions.Compiled);

		private ITexture _Texture = null;
		private SpriteEffect _Effect = SpriteEffect.No;
		private bool _MaintainAspectRation = false;
		private Vector2 _FrameSize = Vector2.Zero;
		private Vector2 _FrameSizePixels = Vector2.Zero;
		private uint _CurrentFrame = 0;
		private float FrameTimeAggregator = 0.0f;
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
				this.Size = this.Size;
			}
		}

		/// <summary>
		/// Liczba klatek animacji na sprite.
		/// </summary>
		public uint FramesCount { get; private set; }

		/// <summary>
		/// Czas trwania jednej klatki.
		/// </summary>
		public float FrameTime { get; private set; }

		/// <summary>
		/// Aktualna klatka.
		/// </summary>
		public uint CurrentFrame
		{
			get { return this._CurrentFrame; }
			set
			{
				while (value >= this.FramesCount)
					value -= this.FramesCount;
				this._CurrentFrame = value;
				this.UpdateTexCoords();
			}
		}

		/// <summary>
		/// Rozmiar klatki.
		/// </summary>
		public Vector2 FrameSize
		{
			get { return this._FrameSizePixels; }
		}

		/// <summary>
		/// Przesuwa animacje.
		/// </summary>
		/// <param name="time">Czas w sekundach.</param>
		public void AdvanceAnimation(double time)
		{
			this.FrameTimeAggregator += (float)time;
			if (this.FrameTimeAggregator > this.FrameTime && this.FramesCount > 1)
			{
				this.CurrentFrame++;
				this.FrameTimeAggregator -= this.FrameTime;
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
				this.UpdateTexture();
				this.UpdateTexCoords();
			}
		}

		/// <summary>
		/// Rozmiar duszka.
		/// </summary>
		public new Vector2 Size
		{
			get { return base.Size; }
			set { base.Size = (this._MaintainAspectRation ? value * System.Math.Min(value.X / this.Texture.Size.X, value.Y / this.Texture.Size.Y) : value); }
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
			: base(position, texture.Size, Vector4.One)
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
			float l = this.Texture.Coordinates.Left   + this._FrameSize.X * this.CurrentFrame,
				  r = this.Texture.Coordinates.Left   + this._FrameSize.X * (this.CurrentFrame + 1),
				  t = this.Texture.Coordinates.Top,
				  b = this.Texture.Coordinates.Top + this._FrameSize.Y;

			float left, right;
			float top, bottom;

			if ((this.Effect & SpriteEffect.FlipHorizontally) == SpriteEffect.FlipHorizontally)
			{
				left = r;
				right = l;
			}
			else
			{
				right = r;
				left = l;
			}

			if ((this.Effect & SpriteEffect.FlipVertically) == SpriteEffect.FlipVertically)
			{
				top = b;
				bottom = t;
			}
			else
			{
				bottom = b;
				top = t;
			}

			this.Vertices[0].TexCoord = new Vector2(left, top);
			this.Vertices[1].TexCoord = new Vector2(right, top);
			this.Vertices[2].TexCoord = new Vector2(right, bottom);
			this.Vertices[3].TexCoord = new Vector2(left, bottom);
		}

		private void UpdateTexture()
		{
			//Ustawiamy na domyślne.
			this.FramesCount = 1;
			this.FrameTime = 1f;
			this.CurrentFrame = 0;
			this._FrameSize = Vector2.One;
			this._FrameSizePixels = Vector2.Zero;

			if (this.Texture != null)
			{
				this._FrameSizePixels = this.Texture.Size;

				var match = AnimationInfoRegex.Match(this.Texture.UserData);
				if (match.Success) //Mamy informacje o animacji
				{
					try
					{
						uint frames = uint.Parse(match.Groups[1].Value);
						Vector2 frameSize = new Vector2((float)uint.Parse(match.Groups[2].Value), (float)uint.Parse(match.Groups[3].Value));
						float framesTime = float.Parse(match.Groups[4].Value, System.Globalization.CultureInfo.InvariantCulture);

						this.FramesCount = frames;
						this.FrameTime = framesTime / frames;
						this._FrameSizePixels = frameSize;
						this._FrameSize.X = frameSize.X / this.Texture.Size.X;
						this._FrameSize.Y = frameSize.Y / this.Texture.Size.Y;
						this.CurrentFrame = 0;
					}
					catch
					{ }
				}
			}
		}
		#endregion
	}
}
