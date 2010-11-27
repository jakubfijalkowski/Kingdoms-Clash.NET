using System.ComponentModel;
using System.Diagnostics;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Interfaces.Graphics.Gui.Objects;
	
	/// <summary>
	/// Obrazek.
	/// </summary>
	[DebuggerDisplay("Image {Texture.Id,nq}")]
	public class Image
		: IImage
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Interfaces.Graphics.Resources.ITexture _Texture = null;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Graphics.Objects.Quad Quad = new Graphics.Objects.Quad(Vector2.Zero, Vector2.Zero, System.Drawing.Color.White);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasSizeSet = false;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasPositionSet = false;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private StretchType _Stretch = StretchType.Fill;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool DoTexCoordsNeedUpdate = true;
		#endregion

		#region IImage Members
		/// <summary>
		/// Tekstura dla obrazka.
		/// </summary>
		[TypeConverter(typeof(Converters.TextureConverter))]
		public Interfaces.Graphics.Resources.ITexture Texture
		{
			get { return this._Texture; }
			set
			{
				this._Texture = value;
				if (this.Stretch == StretchType.Fill && !this.WasSizeSet)
				{
					this.Quad.Size = new Vector2(this._Texture.Width, this._Texture.Height);
				}
				else if (this.Stretch == StretchType.Repeat || this.Stretch == StretchType.RepeatX || this.Stretch == StretchType.RepeatY)
				{
					this.DoTexCoordsNeedUpdate = true;
				}
			}
		}

		/// <summary>
		/// Pozycja.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public Vector2 Position
		{
			get { return this.Quad.Position; }
			set
			{
				this.Quad.Position = value;
				this.WasPositionSet = true;
			}
		}

		/// <summary>
		/// Rozmiar.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public Vector2 Size
		{
			get { return this.Quad.Size; }
			set
			{
				this.Quad.Size = value;
				this.WasSizeSet = true;
			}
		}

		/// <summary>
		/// Typ rozciągania obrazka.
		/// </summary>
		public StretchType Stretch
		{
			get { return this._Stretch; }
			set
			{
				this._Stretch = value;
				this.DoTexCoordsNeedUpdate = true;
			}
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Głębokość, na jakiej znajduje się tekst.
		/// </summary>
		public float Depth
		{
			get { return this.Quad.Depth; }
			set { this.Quad.Depth = value; }
		}

		/// <summary>
		/// Wierzchołki prostokąta, na którym wyświetlany jest tekst.
		/// </summary>
		public Interfaces.Graphics.Vertex[] Vertices
		{
			get
			{
				if (this.DoTexCoordsNeedUpdate)
				{
					this.UpdateTexCoords();
					this.DoTexCoordsNeedUpdate = false;
				}
				return this.Quad.Vertices;
			}
		}

		/// <summary>
		/// Indeksy.
		/// </summary>
		public int[] Indecies
		{
			get { return this.Quad.Indecies; }
		}

		/// <summary>
		/// Kontrolka-rodzic.
		/// </summary>
		public Interfaces.Graphics.Gui.IControl ParentControl { get; set; }

		/// <summary>
		/// Czy obiekt jest widoczny.
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// Poprawia pozycję i/lub rozmiar elementu tak, by pasował do kontrolki.
		/// </summary>
		public void Finish()
		{
			this.DoTexCoordsNeedUpdate = true;
			if (!this.WasPositionSet)
			{
				this.Position = this.ParentControl.Position;
			}
			if (!this.WasSizeSet)
			{
				this.Size = this.ParentControl.Size;
			}
		}
		#endregion

		#region Constructors
		public Image()
		{
			this.Visible = true;
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Uaktualnia koordynaty tekstury.
		/// </summary>
		/// <remarks>
		/// Musimy zastosować tzw. lazy initialization, gdyż metoda ta wymaga dostępu do renderera, którego przy ładowaniu GUI z XAML-a nie mamy. 
		/// </remarks>
		private void UpdateTexCoords()
		{
			float multX = this.ParentControl.Data.Renderer.ViewPortSize.X / this.ParentControl.Data.Renderer.Camera.Size.X;
			float multY = this.ParentControl.Data.Renderer.ViewPortSize.Y / this.ParentControl.Data.Renderer.Camera.Size.Y;

			float realW = this.Size.X * multX;
			float realH = this.Size.Y * multY;

			float x = 1, y = 1;

			switch (this.Stretch)
			{
			case StretchType.RepeatX:
				x = realW / this.Texture.Width;
				break;

			case StretchType.RepeatY:
				y = realH / this.Texture.Height;
				break;

			case StretchType.Repeat:
				x = realW / this.Texture.Width;
				y = realH / this.Texture.Height;
				break;
			}

			this.Quad.Vertices[0].TexCoord = new Vector2(0, 0);
			this.Quad.Vertices[1].TexCoord = new Vector2(x, 0);
			this.Quad.Vertices[2].TexCoord = new Vector2(x, y);
			this.Quad.Vertices[3].TexCoord = new Vector2(0, y);
		}
		#endregion
	}
}
