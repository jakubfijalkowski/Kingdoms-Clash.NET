using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Resources
{
	using Interfaces.Resources;
	
	/// <summary>
	/// Tekstura.
	/// Obsługiwane formaty: BMP, GIF, EXIF, JPG, PNG, TIFF.
	/// Jest thread-safe(ale zależne od kontekstu OpenGL).
	/// </summary>
	/// <remarks>
	/// Musi być używane tylko z wątku, w którym jest aktywny kontekst OpenGL, w innych nie "zadziała".
	/// </remarks>
	public class Texture
		: ITexture
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

		private object PadLock = new object();

		#region IResource Members
		/// <summary>
		/// Identyfikator tekstury.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Plik tekstury.
		/// Najczęściej rózwny Id.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		public Interfaces.ResourcesManager.IResourcesManager Manager { get; set; }
		#endregion

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
			lock (this.PadLock)
			{
				GL.BindTexture(TextureTarget.Texture2D, this.TextureId);
			}
		}
		#endregion

		#region Resource members
		/// <summary>
		/// Ładuje teksturę.
		/// Jeśli nie może załadować używa domyślnej tekstury.
		/// </summary>
		/// <returns>Stan załadowania zasobu.</returns>
		public virtual Interfaces.ResourcesManager.ResourceLoadingState Load()
		{
			lock (this.PadLock)
			{
				Bitmap bm;
				try
				{
					GL.Enable(EnableCap.Texture2D);

					bm = new Bitmap(this.FileName);

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
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		public void Free()
		{
			lock (this.PadLock)
			{
				if (!this.DefaultUsed)
				{
					GL.BindTexture(TextureTarget.Texture2D, 0); //Odbindowujemy jakąkolwiek teksturę - nie przechowujemy nigdzie która jest zbindowana.
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
