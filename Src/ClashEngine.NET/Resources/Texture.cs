using System;
using System.Drawing;

namespace ClashEngine.NET.Resources
{
	using System.Drawing.Imaging;
	using Interfaces.Resources;
	using OpenTK.Graphics.OpenGL;
	
	/// <summary>
	/// Tekstura.
	/// Obsługiwane formaty: BMP, GIF, EXIF, JPG, PNG, TIFF.
	/// </summary>
	public class Texture
		: ResourcesManager.Resource, ITexture
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		#region Default texture
		/// <summary>
		/// Domyślna tekstura. Ładowana gdy nie udało się załadować zasobu.
		/// </summary>
		private static DefaultTexture DefaultTexture = null;

		/// <summary>
		/// Kiedyś musimy zwolnić domyślną teksturę - liczba użyć, jeśli dojdzie do zera - zwalniamy ją.
		/// </summary>
		private static int DefaultTexturesCount = 0;
		#endregion

		/// <summary>
		/// Czy użyto domyślnej tekstury?
		/// </summary>
		private bool DefaultUsed = false;

		#region ITexture members
		/// <summary>
		/// Pobiera identyfikator(OpenGL) tekstury.
		/// </summary>
		public int TextureId { get; protected set; }

		/// <summary>
		/// Szerokość w pikselach.
		/// </summary>
		public int Widgth { get; protected set; }

		/// <summary>
		/// Wysokość w pikselach.
		/// </summary>
		public int Heigth { get; protected set; }

		/// <summary>
		/// Pobiera koordynaty tekstury.
		/// Zawsze stałe - (0.0, 0.0, 1.0, 1.0).
		/// </summary>
		public RectangleF Coordinates
		{
			get
			{
				return new RectangleF(0.0f, 0.0f, 1.0f, 1.0f);
			}
		}

		/// <summary>
		/// Ustawia teksturę jako aktualnie używaną.
		/// </summary>
		public void Bind()
		{
			GL.BindTexture(TextureTarget.Texture2D, this.TextureId);
		}
		#endregion

		#region Resource members
		/// <summary>
		/// Ładuje teksturę.
		/// Jeśli nie może załadować używa domyślnej tekstury.
		/// </summary>
		/// <returns>Stan załadowania zasobu.</returns>
		public override Interfaces.ResourcesManager.ResourceLoadingState Load()
		{
			Bitmap bm;
			try
			{
				bm = new Bitmap(this.Id);

				BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				this.TextureId = GL.GenTexture();
				this.Bind();
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bm.Width, bm.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
				bm.UnlockBits(data);

				//Ustawiamy filtrowanie - w grach 2D linearne nas zadowala.
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				bm.Dispose(); //Od razu zwalniamy, nie mamy potrzeby trzymać tego w pamięci.
			}
			catch (Exception ex)
			{
				Logger.WarnException("Cannot load texture. Using default.", ex);

				if (DefaultTexture == null)
				{
					DefaultTexture = new DefaultTexture();
					DefaultTexture.Load();
				}
				this.DefaultUsed = true;
				this.TextureId = DefaultTexture.TextureId;
				this.Widgth = DefaultTexture.Widgth;
				this.Heigth = DefaultTexture.Heigth;

				++DefaultTexturesCount;

				return Interfaces.ResourcesManager.ResourceLoadingState.DefaultUsed;
			}
			return Interfaces.ResourcesManager.ResourceLoadingState.Success;
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		public override void Free()
		{
			if (!this.DefaultUsed)
			{
				GL.DeleteTexture(this.TextureId);
			}
			else
			{
				--DefaultTexturesCount;
				if (DefaultTexturesCount == 0)
				{
					DefaultTexture.Free();
					DefaultTexture = null;
				}
			}
		}
		#endregion
	}
}
