using System.ComponentModel;
using System.Diagnostics;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Interfaces.Graphics.Gui.Objects;
	
	/// <summary>
	/// Obrazek.
	/// </summary>
	[DebuggerDisplay("Image")]
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
		#endregion

		#region IImage Members
		/// <summary>
		/// Tekstura dla obrazka.
		/// </summary>
		public Interfaces.Graphics.Resources.ITexture Texture
		{
			get { return this._Texture; }
			set
			{
				this._Texture = value;
				if (!this.WasSizeSet)
				{
					this.Quad.Size = new Vector2(this._Texture.Width, this._Texture.Height);
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
			get { return this.Quad.Vertices; }
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
		/// Poprawia pozycję i/lub rozmiar elementu tak, by pasował do kontrolki.
		/// </summary>
		public void Finish()
		{
			this.Quad.Vertices[0].TexCoord = new Vector2(0, 0);
			this.Quad.Vertices[1].TexCoord = new Vector2(1, 0);
			this.Quad.Vertices[2].TexCoord = new Vector2(1, 1);
			this.Quad.Vertices[3].TexCoord = new Vector2(0, 1);

			if (!this.WasPositionSet)
			{
				this.Position = this.ParentControl.Position;
			}
			if (!this.WasSizeSet)
			{
				this.Size = new Vector2(this.Texture.Width, this.Texture.Height);
			}
		}
		#endregion
	}
}
