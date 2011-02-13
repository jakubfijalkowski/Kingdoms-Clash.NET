using OpenTK;

namespace ClashEngine.NET.Graphics.Objects.Internals
{
	using Interfaces.Graphics.Objects;

	/// <summary>
	/// Klasa wewnętrzna reprezentująca tekst z klasy SystemFont.
	/// </summary>
	internal class SystemFontObject
		: IText
	{
		#region Private fields
		private Quad Quad;
		#endregion

		#region IText Members
		public string Content { get; internal set; }

		public Vector2 Position
		{
			get { return this.Quad.Position; }
			set { this.Quad.Position = value; }
		}

		public Vector2 Size
		{
			get { return this.Quad.Size; }
			set { this.Quad.Size = value; }
		}
		#endregion

		#region IObject Members
		public Interfaces.Graphics.Resources.ITexture Texture { get; internal set; }

		public float Depth
		{
			get { return this.Quad.Depth; }
			set { this.Quad.Depth = value; }
		}

		public float Rotation
		{
			get { return this.Quad.Rotation; }
			set { this.Quad.Rotation = value; }
		}

		public Interfaces.Graphics.Vertex[] Vertices { get { return this.Quad.Vertices; } }

		public int[] Indecies { get { return this.Quad.Indecies; } }

		public void PreRender()
		{ }
		#endregion

		#region Constructors
		public SystemFontObject()
		{
			this.Quad = new Quad(Vector2.Zero, Vector2.Zero, System.Drawing.Color.White);
			this.Quad.Vertices[0].TexCoord = new Vector2(0, 0);
			this.Quad.Vertices[1].TexCoord = new Vector2(1, 0);
			this.Quad.Vertices[2].TexCoord = new Vector2(1, 1);
			this.Quad.Vertices[3].TexCoord = new Vector2(0, 1);
		}
		#endregion
	}
}
