using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Extensions;
	using Interfaces.Data;
	using Interfaces.Graphics.Gui.Objects;
	
	/// <summary>
	/// Obrazek.
	/// </summary>
	[DebuggerDisplay("Image {Texture.Id,nq}")]
	public class Image
		: ObjectBase, IDataContext, IImage
	{
		#region Private fields
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
					this.PropertyChanged.Raise(this, () => DataContext);
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
		public Interfaces.Graphics.Resources.ITexture Texture
		{
			get { return this.Quad.Texture; }
			set
			{
				this.Quad.Texture = value;
				if (this.Stretch == StretchType.Fill && !this.WasSizeSet && this.Quad.Texture != null)
				{
					this.Quad.Size = this.Quad.Texture.Size;
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
		public override Vector2 Size
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
		/// Poprawia pozycję i/lub rozmiar elementu tak, by pasował do kontrolki.
		/// </summary>
		public override void OnAdd()
		{
			this.DoTexCoordsNeedUpdate = true;
			if (!this.WasSizeSet)
			{
				this.Size = this.Owner.Size;
			}
		}

		/// <summary>
		/// Wyświetla obiekt.
		/// </summary>
		public override void Render()
		{
			if (this.DoTexCoordsNeedUpdate)
			{
				this.UpdateTexCoords();
				this.DoTexCoordsNeedUpdate = false;
			}
			this.Owner.Data.Renderer.Draw(this.Quad);
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
			float multX = this.Owner.Data.Renderer.Owner.Size.X / this.Owner.Data.Renderer.Camera.Size.X;
			float multY = this.Owner.Data.Renderer.Owner.Size.Y / this.Owner.Data.Renderer.Camera.Size.Y;

			float realW = this.Size.X * multX;
			float realH = this.Size.Y * multY;

			float x = 1, y = 1;

			switch (this.Stretch)
			{
			case StretchType.RepeatX:
				x = realW / this.Texture.Size.X;
				break;

			case StretchType.RepeatY:
				y = realH / this.Texture.Size.Y;
				break;

			case StretchType.Repeat:
				x = realW / this.Texture.Size.X;
				y = realH / this.Texture.Size.Y;
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
