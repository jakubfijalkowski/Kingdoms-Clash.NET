using System.ComponentModel;
using System.Diagnostics;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Objects
{
	using Converters;
	using Graphics.Objects;
	using Interfaces.Graphics.Gui.Objects;

	/// <summary>
	/// Prostokąt.
	/// </summary>
	[DebuggerDisplay("Rectangle")]
	public class Rectangle
		: ObjectBase, IRectangle
	{
		#region Private fields
		private Quad InnerQuad = new Quad(Vector2.Zero, Vector2.Zero, Vector4.One);
		#endregion

		#region IRectangle Members
		/// <summary>
		/// Rozmiar
		/// </summary>
		[TypeConverter(typeof(Vector2Converter))]
		public Vector2 Size
		{
			get { return this.InnerQuad.Size; }
			set { this.InnerQuad.Size = value; }
		}

		/// <summary>
		/// Kolor prostokąta.
		/// </summary>
		[TypeConverter(typeof(Vector4Converter))]
		public Vector4 Color
		{
			get { return this.InnerQuad.Color; }
			set { this.InnerQuad.Color = value; }
		}

		/// <summary>
		/// Głębokość, na której prostokąt się znajduje.
		/// </summary>
		public override float Depth
		{
			get { return this.InnerQuad.Depth; }
			set { this.InnerQuad.Depth = value; }
		}
		#endregion

		#region ObjectBase Members
		/// <summary>
		/// Pozycja absolutna - uwzględnia pozycję(absolutną!) kontrolki(<see cref="ParentControl"/>).
		/// </summary>
		public override Vector2 AbsolutePosition
		{
			get { return this.InnerQuad.Position; }
			protected set { this.InnerQuad.Position = value;}
		}

		public override Interfaces.Graphics.Resources.ITexture Texture { get { return null; } }
		public override Interfaces.Graphics.Vertex[] Vertices { get { return this.InnerQuad.Vertices; } }
		public override int[] Indecies { get { return this.InnerQuad.Indecies; } }

		/// <summary>
		/// Zmieniamy rozmiar i pozycję, jeśli nie zostały zmienione wcześniej.
		/// </summary>
		public override void Finish()
		{
			if (this.Size == Vector2.Zero)
			{
				this.Size = this.ParentControl.Size;
			}
		}
		#endregion

		#region Constructor
		public Rectangle()
		{
			this.Visible = true;
		}
		#endregion
	}
}
