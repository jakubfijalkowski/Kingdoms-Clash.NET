using System.ComponentModel;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Graphics.Objects;
	using Interfaces.Graphics.Gui.Objects;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Tekst.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("Text {TextValue,nq}")]
	public class Text
		: IText, ISupportInitialize
	{
		#region Private fields
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private IFont _Font = null;
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private string _TextValue = string.Empty;
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private Vector4 _Color = new Vector4(0, 0, 0, 1);
		private Graphics.Objects.Quad Quad;
		private bool CustomSize = false;
		private bool WasPositionSet = false;
		#endregion
		
		#region IText Members
		/// <summary>
		/// Czcionka.
		/// </summary>
		public IFont Font
		{
			get { return this._Font; }
			set
			{
				this._Font = value;
				this.UpdateTexture();
			}
		}

		/// <summary>
		/// Tekst.
		/// </summary>
		public string TextValue
		{
			get { return this._TextValue; }
			set
			{
				this._TextValue = value;
				this.UpdateTexture();
			}
		}

		/// <summary>
		/// Kolor tekstu.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector4Converter))]
		public Vector4 Color
		{
			get { return this._Color; }
			set
			{
				this._Color = value;
				this.UpdateTexture();
			}
		}

		/// <summary>
		/// Pozycja.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public Vector2 Position
		{
			get
			{
				return this.Quad.Position;
			}
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
			get
			{
				return this.Quad.Size;
			}
			set
			{
				this.Quad.Size = value;
				this.CustomSize = true;
			}
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Tekstura z tekstem.
		/// </summary>
		public Interfaces.Graphics.Resources.ITexture Texture { get; private set; }

		/// <summary>
		/// Głębokość, na jakiej znajduje się tekst.
		/// </summary>
		public float Depth
		{
			get
			{
				return this.Quad.Depth;
			}
			set
			{
				this.Quad.Depth = value;
			}
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
		/// Aktualizujemy pozycję, jeśli nie była zmieniona.
		/// </summary>
		public void Finish()
		{
			if (!this.WasPositionSet)
			{
				this.Position = this.ParentControl.Position;
			}
		}
		#endregion

		#region ISupportInitialize Members
		public void BeginInit()
		{
			//Quad nie wspiera teksturowania i wszystkie wierzchołki mają TexCoord ustawione na (0,0)
			//musimy to zmienić.
			this.Quad = new Quad(Vector2.Zero, Vector2.Zero, System.Drawing.Color.White);
			this.Quad.Vertices[0].TexCoord = new Vector2(0, 0);
			this.Quad.Vertices[1].TexCoord = new Vector2(1, 0);
			this.Quad.Vertices[2].TexCoord = new Vector2(1, 1);
			this.Quad.Vertices[3].TexCoord = new Vector2(0, 1);
		}

		public void EndInit()
		{
			this.Texture = this.Font.CreateEmptyText();
			this.UpdateTexture();
		}
		#endregion

		#region Private members
		private void UpdateTexture()
		{
			if (this.Texture != null)
			{
				this.Font.DrawString(this.TextValue, this.Color, this.Texture);

				if (!this.CustomSize)
				{
					this.Quad.Size = new Vector2(this.Texture.Width, this.Texture.Height);
				}
			}
		}
		#endregion
	}
}
