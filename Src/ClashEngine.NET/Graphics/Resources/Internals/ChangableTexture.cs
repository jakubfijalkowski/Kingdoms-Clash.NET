using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace ClashEngine.NET.Graphics.Resources.Internals
{
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Wewnętrzna klasa dla tekstury, którą można zmieniać po załadowaniu.
	/// </summary>
	internal class ChangableTexture
		: ITexture
	{
		#region Private fields
		private static readonly RectangleF _Coordinates = new RectangleF(0, 0, 1, 1);
		private object PadLock = new object();
		#endregion

		#region ITexture Members
		public int TextureId { get; private set; }
		public Vector2 Size { get; private set; }
		public System.Drawing.RectangleF Coordinates { get { return _Coordinates; } }
		public string UserData
		{
			get { return string.Empty; }
		}

		public void Bind()
		{
			lock (this.PadLock)
			{
				GL.BindTexture(TextureTarget.Texture2D, this.TextureId);
			}
		}
		#endregion

		#region IResource Members
		public string Id { get; set; }
		public string FileName
		{
			get { return this.Id; }
			set { this.Id = value; }
		}
		public Interfaces.IResourcesManager Manager { get; set; }

		/// <summary>
		/// Nic nie robi.
		/// </summary>
		/// <returns></returns>
		public Interfaces.ResourceLoadingState Load()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Usuwa teksturę.
		/// </summary>
		public void Free()
		{
			lock (this.PadLock)
			{
				GL.BindTexture(TextureTarget.Texture2D, 0); //Odbindowujemy jakąkolwiek teksturę - nie przechowujemy nigdzie która jest zbindowana.
				GL.DeleteTexture(this.TextureId);
			}
		}
		#endregion

		#region Constructors
		public ChangableTexture()
		{
			this.TextureId = 0;
			this.Size = Vector2.Zero;
		}
		#endregion

		#region Managing
		/// <summary>
		/// Podmienia teksturę.
		/// </summary>
		/// <param name="bm">Nowa tekstura.</param>
		public void Set(Bitmap bm)
		{
			if (this.TextureId == 0)
			{
				this.TextureId = GL.GenTexture();
			}

			this.Size = new Vector2(bm.Width, bm.Height);

			BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			this.Bind();
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bm.Width, bm.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			bm.UnlockBits(data);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Free();
		}
		#endregion
	}
}
