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
		private Vector2 _Size = new Vector2(0, 0);
		private Interfaces.Graphics.Objects.IText TextObject = Resources.SystemFont.CreateEmptyObject();
		private bool Initialized = false;
		private object _DataContext = null;
		private bool DoTextureNeedUpdate = false;
		private bool WasSizeSet = false;
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
		public override Vector2 Size
		{
			get { return this._Size; }
			set
			{
				if (this._Size != value)
				{
					this._Size = value;
					if (this._Size.X == 0 && this._Size.Y == 0)
					{
						this.WasSizeSet = true;
						this.DoTextureNeedUpdate = true;
					}
					this.Owner.Layout();
				}
			}
		}

		/// <summary>
		/// Prawdziwy rozmiar
		/// </summary>
		public Vector2 RealSize
		{
			get { return this.TextObject.Size; }
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
		/// Głębokość, na jakiej znajduje się tekst.
		/// </summary>
		public override float Depth
		{
			get { return this.TextObject.Depth; }
			set { this.TextObject.Depth = value; }
		}

		/// <summary>
		/// Aktualizujemy pozycję, jeśli nie była zmieniona.
		/// </summary>
		public override void OnAdd()
		{
			this.Initialized = true;
			if (this.Size == Vector2.Zero)
			{
				this.Size = this.Owner.Size;
			}
			this.DoTextureNeedUpdate = true;
		}

		/// <summary>
		/// Wyświetla obiekt.
		/// </summary>
		public override void Render()
		{
			if (this.DoTextureNeedUpdate)
			{
				this.UpdateTexture();
				this.DoTextureNeedUpdate = false;
			}
			this.Owner.Data.Renderer.Draw(this.TextObject);
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

				if (!this.WasSizeSet)
				{
					this.Font.Draw(this.TextValue, this.Color, this.TextObject);
					this._Size.X = this.TextObject.Texture.Size.X / multX;
					this._Size.Y = this.TextObject.Texture.Size.Y / multY;
					this.TextObject.Size = this._Size;
					this.Owner.Layout();
				}
				else
				{
					this.Font.Draw(this.TextValue, this.Color,
					   new System.Drawing.RectangleF(0f, 0f, this.Size.X * multX, this.Size.Y * multY),
						   this.TextObject);
				}
			}
		}
		#endregion
	}
}
