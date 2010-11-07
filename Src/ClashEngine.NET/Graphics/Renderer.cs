using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Graphics
{
	using Interfaces.Graphics;

	/// <summary>
	/// Renderer.
	/// Używa GL.Begin i GL.End.
	/// </summary>
	public class Renderer
		: IRenderer
	{
		#region Private fields
		private ObjectComparer Comparer = new ObjectComparer(SortMode.Texture);
		private SortedList<IObject, object> Objects;
		#endregion

		#region IRenderer Members
		/// <summary>
		/// Tryb sortowania.
		/// </summary>
		public SortMode SortMode
		{
			get { return this.Comparer.SortMode; }
			set { this.Comparer.SortMode = value; }
		}

		/// <summary>
		/// Rysuje obiekt.
		/// </summary>
		/// <param name="obj">Obiekt do odrysowania.</param>
		public void Draw(IObject obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			this.Objects.Add(obj, null);
		}

		/// <summary>
		/// Wyświetla wszystkie rysowane obiekty(<see cref="Draw"/>).
		/// </summary>
		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			foreach (var obj in this.Objects)
			{
				if (obj.Key.Texture != null)
				{
					obj.Key.Texture.Bind();
				}
				else
				{
					GL.BindTexture(TextureTarget.Texture2D, 0);
				}

				GL.Begin(BeginMode.Triangles);
				if (obj.Key.Indecies != null)
				{
					foreach (var idx in obj.Key.Indecies)
					{
						GL.Color4(obj.Key.Vertices[idx].Color);
						GL.TexCoord2(obj.Key.Vertices[idx].TexCoord);
						GL.Vertex2(obj.Key.Vertices[idx].Position);
					}
				}
				else
				{
					foreach (var v in obj.Key.Vertices)
					{
						GL.Color4(v.Color);
						GL.TexCoord2(v.TexCoord);
						GL.Vertex2(v.Position);
					}
				}
				GL.End();
			}
			this.Objects.Clear();
		}
		#endregion

		#region Constructors
		public Renderer()
		{
			this.Objects = new SortedList<IObject, object>(this.Comparer);
		}
		#endregion

		#region Comparer
		private class ObjectComparer
			: IComparer<IObject>
		{
			public SortMode SortMode { get; set; }

			public ObjectComparer(SortMode mode)
			{
				this.SortMode = mode;
			}

			#region IComparer<IObject> Members
			public int Compare(IObject x, IObject y)
			{
				switch (this.SortMode)
				{
				case SortMode.Texture:
					int cmp = (x.Texture != null && y.Texture != null ? x.Texture.GetHashCode().CompareTo(y.Texture.GetHashCode()) : 0);
					if (cmp == 0)
					{
						return (x.Depth < y.Depth ? 1 : -1);
					}
					return cmp;

				case SortMode.FrontToBack:
					return (x.Depth < y.Depth ? 1 : -1);

				case SortMode.BackToFront:
					return (x.Depth >= y.Depth ? 1 : -1);
				}
				return 0;
			}
			#endregion
		}
		#endregion
	}
}
