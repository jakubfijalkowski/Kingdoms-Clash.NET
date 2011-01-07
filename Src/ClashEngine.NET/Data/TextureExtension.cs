using System;
using System.Windows.Markup;
using System.Xaml;

namespace ClashEngine.NET.Data
{
	using Graphics.Resources;
	using Interfaces.Data;

	/// <summary>
	/// Rozszerzenie XAML pobierające teksturę z managera zasobów kontenera GUI.
	/// </summary>
	[MarkupExtensionReturnType(typeof(Interfaces.Graphics.Resources.ITexture))]
	public class TextureExtension
		: MarkupExtension, ITextureExtension
	{
		#region ITextureExtension Members
		/// <summary>
		/// Ścieżka do obrazka z teksturą.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Jeśli jest niepuste określa Id w atlasie tekstur.
		/// </summary>
		public string TextureId { get; set; }
		#endregion

		#region MarkupExtension Members
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootProvider == null)
			{
				throw new InvalidOperationException("IRootObjectProvider");
			}
			var rootObject = rootProvider.RootObject as Interfaces.Graphics.Gui.IXamlGuiContainer;
			if (rootObject == null)
			{
				throw new InvalidOperationException("RootObject");
			}

			if (string.IsNullOrEmpty(this.TextureId))
			{
				return rootObject.Manager.Load<Texture>(this.Path);
			}
			else
			{
				return rootObject.Manager.Load<TexturesAtlas>(this.Path)[this.TextureId];
			}
		}
		#endregion

		#region Constructors
		public TextureExtension()
		{ }

		/// <summary>
		/// Inicjalizuje rozszerzenie.
		/// </summary>
		public TextureExtension(string texture)
		{
			this.Path = texture;
		}
		#endregion
	}
}
