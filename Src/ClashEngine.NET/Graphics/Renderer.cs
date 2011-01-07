using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Graphics
{
	using Interfaces;
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
		private bool IsRunning = false;
		private ICamera _Camera = null;
		private ICamera _DefaultCamera = null;
		#endregion

		#region IRenderer Members
		/// <summary>
		/// Okno, na którym renderer będzie wyświetlał elementy.
		/// </summary>
		public IWindow Owner { get; private set; }

		/// <summary>
		/// Tryb sortowania.
		/// </summary>
		public SortMode SortMode
		{
			get { return this.Comparer.SortMode; }
			set { this.Comparer.SortMode = value; }
		}

		/// <summary>
		/// Kamera.
		/// </summary>
		/// <remarks>
		/// Jeśli kamera jest zmieniana w trakcie pracy(pomiędzy Begin i End) przed zmianą zostaje opróżniony bufor obiektów.
		/// </remarks>
		public ICamera Camera
		{
			get { return (this._Camera == this.DefaultCamera ? null : this._Camera); }
			set
			{
				if (this._Camera != value)
				{
					if (this.IsRunning)
					{
						this.Flush();
					}
					if (value == null)
					{
						this._Camera = this.DefaultCamera;
					}
					else
					{
						this._Camera = value;
					}
					this.UpdateMatricies();
					this.Camera.NeedUpdate = false;
				}
			}
		}

		/// <summary>
		/// Domyślna kamera. Używana, gdy <see cref="Camera"/> == null.
		/// </summary>
		public ICamera DefaultCamera
		{
			get { return this._DefaultCamera; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.Camera == null)
				{
					this.Camera = value;
				}
				this._DefaultCamera = value;
			}
		}

		/// <summary>
		/// Rysuje obiekt.
		/// Musi być wywołana pomiędzy <see cref="Begin"/> i <see cref="End"/>.
		/// </summary>
		/// <param name="obj">Obiekt do odrysowania.</param>
		public void Draw(IObject obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!this.IsRunning)
			{
				throw new InvalidOperationException("Must be called between Begin and End");
			}
			this.Objects.Add(obj, null);
		}

		/// <summary>
		/// Rozpoczyna rysowanie.
		/// </summary>
		public void Begin()
		{
			this.IsRunning = true;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		/// <summary>
		/// Kończy rysowanie.
		/// </summary>
		public void End()
		{
			this.Flush();
			this.IsRunning = false;
		}

		/// <summary>
		/// Opróżnia renderer.
		/// Musi być wywołana pomiędzy <see cref="Begin"/> i <see cref="End"/>.
		/// </summary>
		public void Flush()
		{
			if (!this.IsRunning)
			{
				throw new InvalidOperationException("Must be called between Begin and End");
			}

			if (this.Camera != null && this.Camera.NeedUpdate)
			{
				this.UpdateMatricies();
				this.Camera.NeedUpdate = false;
			}

			foreach (var obj in this.Objects)
			{
				//obj.Key.PreRender();
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
		/// <summary>
		/// Inicjalizuje renderer.
		/// </summary>
		/// <param name="owner">Okno, na którym renderer będzie wyświetlał elementy.</param>
		public Renderer(IWindow owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.Objects = new SortedList<IObject, object>(this.Comparer);
			this.Owner = owner;
			this._Camera = this._DefaultCamera = new Cameras.Movable2DCamera(owner.Size, new System.Drawing.RectangleF(0, 0, owner.Size.X, owner.Size.Y));
		}
		#endregion

		#region Private methods
		private void UpdateMatricies()
		{
			GL.MatrixMode(MatrixMode.Projection);
			var proj = this.Camera.ProjectionMatrix;
			GL.LoadMatrix(ref proj);

			GL.MatrixMode(MatrixMode.Modelview);
			var view = this.Camera.ViewMatrix;
			GL.LoadMatrix(ref view);
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
