using OpenTK;

namespace ClashEngine.NET.Graphics.Resources.Internals
{
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Wewnętrzna tekstura atlasu.
	/// </summary>
	internal class AtlasTexture
		: ITexture
	{
		#region Private fields
		private ITexture InnerTexture;
		#endregion

		#region ITexture Members
		/// <summary>
		/// Identyfikator tekstury.
		/// </summary>
		public int TextureId
		{
			get { return this.InnerTexture.TextureId; }
		}

		/// <summary>
		/// Rozmiar(w pikselach) tekstury.
		/// </summary>
		public Vector2 Size { get; private set; }

		/// <summary>
		/// Koordynaty tekstury w atlasie.
		/// </summary>
		public System.Drawing.RectangleF Coordinates { get; private set; }

		/// <summary>
		/// Dane użytkownika - pobierane z głównej tekstury.
		/// </summary>
		public string UserData
		{
			get { return this.InnerTexture.UserData; }
		}

		/// <summary>
		/// Binduje wewnętrzną teksturę.
		/// </summary>
		public void Bind()
		{
			this.InnerTexture.Bind();
		}
		#endregion

		#region IResource Members
		/// <summary>
		/// Identyfikator - NazwaAtlasu/Identyfikator.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Identyfikator - NazwaAtlasu/Identyfikator.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Manager atlasu.
		/// </summary>
		public Interfaces.IResourcesManager Manager { get; set; }

		/// <summary>
		/// Nieużywane.
		/// </summary>
		/// <returns></returns>
		public Interfaces.ResourceLoadingState Load()
		{
			return Interfaces.ResourceLoadingState.Success;
		}

		/// <summary>
		/// Nieużywane.
		/// </summary>
		public void Free()
		{ }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje obiekt.
		/// </summary>
		/// <param name="inner">Wewnętrzna tekstura.</param>
		/// <param name="rect">Koordynaty tekstury w atlasie.</param>
		/// <param name="id">Identyfikator.</param>
		/// <param name="atlasName">Nazwa atlasu.</param>
		public AtlasTexture(ITexture inner, System.Drawing.RectangleF rect, string id, string atlasName)
		{
			this.Id = this.FileName = string.Format("{0}/{1}", atlasName, id);
			this.Coordinates = rect;
			this.Size = new Vector2(inner.Size.X * rect.Width, inner.Size.Y * rect.Height);
			this.InnerTexture = inner;
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{ }
		#endregion
	}
}
