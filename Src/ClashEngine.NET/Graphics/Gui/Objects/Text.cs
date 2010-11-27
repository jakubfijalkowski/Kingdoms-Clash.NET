using System.ComponentModel;
using System.Diagnostics;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Interfaces.Graphics.Gui.Objects;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Tekst.
	/// </summary>
	[DebuggerDisplay("Text {TextValue}")]
	public class Text
		: IText
	{
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IFont _Font = null;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string _TextValue = string.Empty;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector4 _Color = new Vector4(0, 0, 0, 1);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Interfaces.Graphics.Objects.IText TextObject = Resources.SystemFont.CreateEmptyObject();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasSizeSet = false;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool WasPositionSet = false;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool Initialized = false;
		#endregion

		#region IText Members
		/// <summary>
		/// Czcionka.
		/// </summary>
		[TypeConverter(typeof(Converters.FontConverter))]
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
			get { return this.TextObject.Position; }
			set
			{
				this.TextObject.Position = value;
				this.WasPositionSet = true;
			}
		}

		/// <summary>
		/// Rozmiar.
		/// </summary>
		[TypeConverter(typeof(Converters.Vector2Converter))]
		public Vector2 Size
		{
			get { return this.TextObject.Size; }
			set
			{
				this.TextObject.Size = value;
				this.WasSizeSet = true;
			}
		}
		#endregion

		#region IObject Members
		/// <summary>
		/// Tekstura z tekstem.
		/// </summary>
		public Interfaces.Graphics.Resources.ITexture Texture { get { return this.TextObject.Texture; } }

		/// <summary>
		/// Głębokość, na jakiej znajduje się tekst.
		/// </summary>
		public float Depth
		{
			get { return this.TextObject.Depth; }
			set { this.TextObject.Depth = value; }
		}

		/// <summary>
		/// Wierzchołki prostokąta, na którym wyświetlany jest tekst.
		/// </summary>
		public Interfaces.Graphics.Vertex[] Vertices
		{
			get { return this.TextObject.Vertices; }
		}

		/// <summary>
		/// Indeksy.
		/// </summary>
		public int[] Indecies
		{
			get { return this.TextObject.Indecies; }
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
		/// Aktualizujemy pozycję, jeśli nie była zmieniona.
		/// </summary>
		public void Finish()
		{
			this.Initialized = true;
			if (!this.WasPositionSet)
			{
				this.Position = this.ParentControl.Position;
			}
			if (!this.WasSizeSet)
			{
				this.Size = this.ParentControl.Size;
			}
			this.UpdateTexture();
		}
		#endregion

		#region Constructors
		public Text()
		{
			this.Visible = true;
		}
		#endregion

		#region Private members
		private void UpdateTexture()
		{
			if (this.Initialized)
			{
				this.Font.Draw(this.TextValue, this.Color,
					new System.Drawing.RectangleF(0f, 0f, this.Size.X, this.Size.Y),
						this.TextObject);
			}
		}
		#endregion
	}
}
