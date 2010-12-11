using System.ComponentModel;
using System.Diagnostics;
using OpenTK;
using System.Windows.Markup;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Interfaces.Graphics.Gui.Objects;
	using Interfaces.Data;
	
	/// <summary>
	/// Obrazek.
	/// </summary>
	[DebuggerDisplay("Image {Texture.Id,nq}")]
	public class Image
		: ObjectBase, IDataContext, IImage
	{
		#region Private fields
		private Interfaces.Graphics.Resources.ITexture _Texture = null;
		private Graphics.Objects.Quad Quad = new Graphics.Objects.Quad(Vector2.Zero, Vector2.Zero, System.Drawing.Color.White);
		private bool WasSizeSet = false;
		private StretchType _Stretch = StretchType.Fill;
		private bool DoTexCoordsNeedUpdate = true;
		private object _DataContext = null;
		#endregion

		#region IDataContext Members
		/// <summary>
		/// Kontekst danych.
		/// </summary>
		[TypeConverter(typeof(NameReferenceConverter))]
		public object DataContext
		{
			get { return this._DataContext; }
			set
			{
				if (value != this._DataContext)
				{
					this._DataContext = value;
					if (this.PropertyChanged != null)
					{
						this.PropertyChanged(this, new PropertyChangedEventArgs("DataContext"));
					}
				}
			}
		}

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie którejś z właściwości.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion
		#endregion

		#region IImage Members
		/// <summary>
		/// Tekstura dla obrazka.
		/// </summary>
		[TypeConverter(typeof(Converters.TextureConverter))]
		public override Interfaces.Graphics.Resources.ITexture Texture
		{
			get { return this._Texture; }
			set
			{
				this._Texture = value;
				if (this.Stretch == StretchType.Fill && !this.WasSizeSet && this._Texture != null)
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

		#region ObjectBase Members
		/// <summary>
		/// Pozycja absolutna - uwzględnia pozycję(absolutną!) kontrolki(<see cref="ParentControl"/>).
		/// </summary>
		public override Vector2 AbsolutePosition
		{
			get { return this.Quad.Position; }
			protected set { this.Quad.Position = value; }
		}

		/// <summary>
		/// Głębokość, na jakiej znajduje się tekst.
		/// </summary>
		public override float Depth
		{
			get { return this.Quad.Depth; }
			set { this.Quad.Depth = value; }
		}

		/// <summary>
		/// Wierzchołki prostokąta, na którym wyświetlany jest tekst.
		/// </summary>
		public override Interfaces.Graphics.Vertex[] Vertices
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
		public override int[] Indecies
		{
			get { return this.Quad.Indecies; }
		}

		/// <summary>
		/// Poprawia pozycję i/lub rozmiar elementu tak, by pasował do kontrolki.
		/// </summary>
		public override void Finish()
		{
			this.DoTexCoordsNeedUpdate = true;
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
