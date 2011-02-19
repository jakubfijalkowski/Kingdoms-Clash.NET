using OpenTK;

namespace ClashEngine.NET.Graphics.Objects
{
	using Extensions;
	using Interfaces.Graphics;
	using Interfaces.Graphics.Objects;

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
		private RotationPointSettings _RotationPointSettings = RotationPointSettings.TopLeft;
		#endregion

		#region IQuad Members
		/// <summary>
		/// Pozycja prostokąta.
		/// </summary>
		public Vector2 Position
		{
			get { return this._Vertices[0].Position; }
			set { this.UpdatePositions(value, this.Size); }
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
			set { this.UpdatePositions(this.Position, value); }
		}

		/// <summary>
		/// Kolor prostokąta.
		/// </summary>
		public OpenTK.Vector4 Color
		{
			get { return this.Vertices[0].Color; }
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
		/// Tekstura.
		/// </summary>
		public Interfaces.Graphics.Resources.ITexture Texture { get; set; }

		/// <summary>
		/// Głębokość, na której znajduje się obiekt.
		/// </summary>
		public float Depth { get; set; }

		/// <summary>
		/// Rotacja obiektu.
		/// </summary>
		public float Rotation { get; set; }

		/// <summary>
		/// Punkt, w którym będziemy obracać nasz obiekt.
		/// </summary>
		public Vector2 RotationPoint { get; set; }

		/// <summary>
		/// Ustawienia automatycznego ustawiania punktu rotacji.
		/// </summary>
		/// <remarks>
		/// Przy używaniu <see cref="RotationPointSettings.Custom"/> należy ręcznie ustawić <see cref="RotationPoint"/>.
		/// Jeśli są inne ustawienia - nie zmieniać <see cref="RotationPoint"/> ręcznie.
		/// </remarks>
		public RotationPointSettings RotationPointSettings
		{
			get { return this._RotationPointSettings; }
			set
			{
				if (this.RotationPointSettings != value)
				{ this._RotationPointSettings = value; this.UpdatePositions(this.Position, this.Size); }
			}
		}

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

			switch (this.RotationPointSettings)
			{
				case RotationPointSettings.TopLeft:
					this.RotationPoint = this.Vertices[0].Position;
					break;
				case RotationPointSettings.BottomLeft:
					this.RotationPoint = this.Vertices[3].Position;
					break;
				case RotationPointSettings.TopRight:
					this.RotationPoint = this.Vertices[1].Position;
					break;
				case RotationPointSettings.BottomRight:
					this.RotationPoint = this.Vertices[2].Position;
					break;
				case RotationPointSettings.Center:
					this.RotationPoint = this.Vertices[0].Position + new Vector2(
						(this.Vertices[1].Position.X - this.Vertices[0].Position.X) / 2f,
						(this.Vertices[3].Position.Y - this.Vertices[0].Position.Y) / 2f);
					break;
			}
		}
		#endregion
	}
}
