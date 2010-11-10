using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

namespace ClashEngine.NET.Graphics.Resources
{
	using Interfaces.Graphics.Resources;
	using Internals;

	/// <summary>
	/// Atlas tekstur.
	/// </summary>
	[DebuggerDisplay("Textures atlas {Id,nq}")]
	public class TexturesAtlas
		: ITexturesAtlas
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");
		
		#region Private fields
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		private Dictionary<string, ITexture> Textures = new Dictionary<string, ITexture>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ITexture InnerTexture;
		private bool DefaultUsed = false;
		#endregion

		#region ITexturesAtlas Members
		/// <summary>
		/// Kolekcja z identyfikatorami tekstur, które znajdują się w atlasie.
		/// Tylko do odczytu.
		/// </summary>
		public ICollection<string> IDs
		{
			get { return this.Textures.Keys; }
		}

		/// <summary>
		/// Pobiera teksturę o wskazanum Id.
		/// </summary>
		/// <param name="id">Identyfikator tekstury.</param>
		/// <returns>Tekstura lub null, gdy nie znaleziono.</returns>
		public ITexture this[string id]
		{
			get
			{
				ITexture tex;
				if (this.InnerTexture == DefaultTexture.Instance)
				{
					return this.InnerTexture;
				}
				else if (this.Textures.TryGetValue(id, out tex))
				{
					return tex;
				}
				else if (!this.DefaultUsed)
				{
					DefaultTexture.Instance.Load();
				}
				Logger.Warn("Texture {0} in atlas {1} not found. Using default", id, this.Id);
				return DefaultTexture.Instance;
			}
		}
		#endregion

		#region IResource Members
		/// <summary>
		/// Identyfikator zasobu.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Ścieżka do zasobu.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		public Interfaces.IResourcesManager Manager { get; set; }

		/// <summary>
		/// Ładuje atlas tekstur z pliku XML.
		/// </summary>
		/// <returns></returns>
		public Interfaces.ResourceLoadingState Load()
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(this.FileName);
				
				string image = string.Empty;
				if (doc.DocumentElement.HasAttribute("image"))
				{
					image = doc.DocumentElement.GetAttribute("image");
				}
				else
				{
					throw new XmlException("Attribute image is required");
				}
				
				Texture tex = new Texture();
				tex.Id = image;
				tex.FileName = System.IO.Path.Combine(this.Manager.ContentDirectory, image);
				tex.Load();
				this.InnerTexture = tex;

				foreach (XmlElement item in doc.DocumentElement.GetElementsByTagName("image"))
				{
					string id = item.GetAttribute("id").Trim();
					if (string.IsNullOrWhiteSpace(id))
					{
						Logger.Warn("Attribute id in image element is missing");
						continue;
					}

					string[] rect = item.InnerText.Split(',');
					float left, top, right, bottom;
					if (rect.Length != 4
						|| !float.TryParse(rect[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out left)   || left < 0   || left > 1
						|| !float.TryParse(rect[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out top)    || top < 0    || top > 1
						|| !float.TryParse(rect[2].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out right)  || right < 0  || right > 1
						|| !float.TryParse(rect[3].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out bottom) || bottom < 0 || bottom > 1
						)
					{
						Logger.Warn("Value of image {0} isn't valid rectangle", id);
						continue;
					}

					this.Textures.Add(id, new AtlasTexture(this.InnerTexture, System.Drawing.RectangleF.FromLTRB(left, top, right, bottom), id, this.Id));
				}
			}
			catch (System.Exception ex)
			{
				this.InnerTexture = DefaultTexture.Instance;
				Logger.WarnException("Cannot load textures atlas. Using default.", ex);
				return Interfaces.ResourceLoadingState.DefaultUsed;
			}
			return Interfaces.ResourceLoadingState.Success;
		}

		/// <summary>
		/// Zwalania atlas.
		/// </summary>
		public void Free()
		{
			this.Textures.Clear();
			this.InnerTexture.Free();
			if (this.DefaultUsed)
			{
				DefaultTexture.Instance.Free();
			}
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			this.Free();
		}
		#endregion

		#region IEnumerable<ITexture> Members
		public IEnumerator<ITexture> GetEnumerator()
		{
			return this.Textures.Values.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Textures.Values.GetEnumerator();
		}
		#endregion
	}
}
