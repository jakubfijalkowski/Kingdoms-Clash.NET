using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Graphics.Resources
{
	using Interfaces.Graphics.Resources;
	using Internals;

	/// <summary>
	/// Tekstura.
	/// Obsługiwane formaty: BMP, GIF, EXIF, JPG, PNG, TIFF.
	/// Jest thread-safe(ale zależne od kontekstu OpenGL).
	/// </summary>
	/// <remarks>
	/// Musi być używane tylko z wątku, w którym jest aktywny kontekst OpenGL, w innych nie "zadziała".
	/// </remarks>
	[System.Diagnostics.DebuggerDisplay("Texture {Id,nq}")]
	public class Texture
		: ITexture
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		private static readonly int[] Properties = new int[] { 0x0320, 0x010E, 0x9286, 0x0131 };
		private static readonly Regex UserDataRegex = new Regex(@"\[(.+)\]", RegexOptions.Compiled);

		#region Private fields
		/// <summary>
		/// Czy użyto domyślnej tekstury?
		/// </summary>
		private bool DefaultUsed = false;

		private object PadLock = new object();
		#endregion

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
		public Interfaces.IResourcesManager Manager { get; set; }

		/// <summary>
		/// Ładuje teksturę.
		/// Jeśli nie może załadować używa domyślnej tekstury.
		/// </summary>
		/// <returns>Stan załadowania zasobu.</returns>
		public virtual Interfaces.ResourceLoadingState Load()
		{
			lock (this.PadLock)
			{
				try
				{
					this.UserData = string.Empty;

					GL.Enable(EnableCap.Texture2D);

					using (Bitmap bm = new Bitmap(this.FileName))
					{
						BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

						this.Size = new Vector2(bm.Width, bm.Height);

						this.TextureId = GL.GenTexture();
						this.Bind();
						GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bm.Width, bm.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
						bm.UnlockBits(data);

						//Pobieramy dane z właściwości
						PropertyItem property = null;
						foreach (var propId in Properties)
						{
							try
							{
								property = bm.GetPropertyItem(propId); //PropertyTagImageTitle
								break;
							}
							catch
							{ }
						}
						Match match;
						if((property != null && (match = UserDataRegex.Match(Encoding.Default.GetString(property.Value))).Success) ||
							(match = UserDataRegex.Match(this.Id)).Success)
						{
							this.UserData = match.Groups[1].Value;
						}
					}

					//Ustawiamy filtrowanie - w grach 2D linearne nas zadowala.
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
					GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				}
				catch (Exception ex)
				{
					Logger.WarnException("Cannot load texture. Using default.", ex);

					DefaultTexture.Instance.Load();
					this.DefaultUsed = true;
					this.TextureId = DefaultTexture.Instance.TextureId;
					this.Size = DefaultTexture.Instance.Size;

					return Interfaces.ResourceLoadingState.DefaultUsed;
				}
				return Interfaces.ResourceLoadingState.Success;
			}
		}

		/// <summary>
		/// Zwalnia zasób.
		/// </summary>
		public virtual void Free()
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
					DefaultTexture.Instance.Free();
				}
			}
		}
		#endregion

		#region ITexture Members
		/// <summary>
		/// Pobiera identyfikator(OpenGL) tekstury.
		/// </summary>
		public int TextureId { get; protected set; }

		/// <summary>
		/// Rozmiar(w pikselach) tekstury.
		/// </summary>
		public Vector2 Size { get; protected set; }

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
		/// Dane wpisane przez użytkownika.
		/// Powinny być pobierane z(w takiej kolejności):
		///		PropertyTagImageTitle
		///		PropertyTagImageDescription
		///		PropertyTagExifUserComment
		///		PropertyTagSoftwareUsed
		///	Jeśli nie istnieją - powinny być pobrane z nazwy pliku(pomiędzy [ i ], włącznie z [ i ]);
		/// </summary>
		public string UserData { get; private set; }

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

		#region IDisposable Members
		public void Dispose()
		{
			this.Free();
		}
		#endregion

		#region Object Members
		public override int GetHashCode()
		{
			return this.Id.GetHashCode() + this.TextureId.GetHashCode();
		}
		#endregion
	}
}
