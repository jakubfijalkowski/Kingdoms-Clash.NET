using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Markup;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Extensions;
	using Interfaces.Data;
	using Interfaces.Graphics.Gui.Objects;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Tekst.
	/// </summary>
	[DebuggerDisplay("Text {TextValue}")]
	public class Text
		: ObjectBase, IDataContext, IText
	{
		#region Private fields
		private IFont _Font = null;
		private string _TextValue = string.Empty;
		private Vector4 _Color = new Vector4(0, 0, 0, 1);
		private Interfaces.Graphics.Objects.IText TextObject = Resources.SystemFont.CreateEmptyObject();
		private bool Initialized = false;
		private object _DataContext = null;
		private bool DoTextureNeedUpdate = false;
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
				this.DoTextureNeedUpdate = true;
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
				this.DoTextureNeedUpdate = true;
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
				this.DoTextureNeedUpdate = true;
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
			}
		}
		#endregion

		#region ObjectBase Members
		/// <summary>
		/// Pozycja absolutna - uwzględnia pozycję(absolutną!) kontrolki(<see cref="ParentControl"/>).
		/// </summary>
		public override Vector2 AbsolutePosition
		{
			get { return this.TextObject.Position; }
			protected set { this.TextObject.Position = value; }
		}

		/// <summary>
		/// Tekstura z tekstem.
		/// </summary>
		public override Interfaces.Graphics.Resources.ITexture Texture
		{
			get { return this.TextObject.Texture; }
			set { }
		}

		/// <summary>
		/// Głębokość, na jakiej znajduje się tekst.
		/// </summary>
		public override float Depth
		{
			get { return this.TextObject.Depth; }
			set { this.TextObject.Depth = value; }
		}

		/// <summary>
		/// Wierzchołki prostokąta, na którym wyświetlany jest tekst.
		/// </summary>
		public override Interfaces.Graphics.Vertex[] Vertices
		{
			get
			{
				return this.TextObject.Vertices;
			}
		}

		/// <summary>
		/// Indeksy.
		/// </summary>
		public override int[] Indecies
		{
			get { return this.TextObject.Indecies; }
		}

		/// <summary>
		/// Aktualizujemy pozycję, jeśli nie była zmieniona.
		/// </summary>
		public override void Finish()
		{
			this.Initialized = true;
			if (this.Size == Vector2.Zero)
			{
				this.Size = this.Owner.Size;
			}
			this.DoTextureNeedUpdate = true;
		}

		/// <summary>
		/// Wywoływane przed wyrenderowaniem obiektu.
		/// </summary>
		public override void PreRender()
		{
			if (this.DoTextureNeedUpdate)
			{
				this.UpdateTexture();
				this.DoTextureNeedUpdate = false;
			}
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
				float multX = this.Owner.Data.Renderer.Owner.Size.X / this.Owner.Data.Renderer.Camera.Size.X;
				float multY = this.Owner.Data.Renderer.Owner.Size.Y / this.Owner.Data.Renderer.Camera.Size.Y;
				this.Font.Draw(this.TextValue, this.Color,
					new System.Drawing.RectangleF(0f, 0f, this.Size.X * multX, this.Size.Y * multY),
						this.TextObject);
			}
		}
		#endregion
	}
}
