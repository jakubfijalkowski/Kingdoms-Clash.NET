using System;
using System.Windows.Markup;
using System.Xaml;

namespace ClashEngine.NET.Data
{
	using Interfaces;
	using Interfaces.Data;

	/// <summary>
	/// Rozszerzenie XAML - czcionka.
	/// </summary>
	[MarkupExtensionReturnType(typeof(Graphics.Resources.SystemFont))]
	public class SystemFontExtension
		: MarkupExtension, IFontExtension
	{
		#region IFontExtension Members
		/// <summary>
		/// Nazwa czcionki.
		/// </summary>
		public string Font { get; set; }
		#endregion

		#region MarkupExtension Members
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			IRootObjectProvider rootProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if(rootProvider == null)
			{
				throw new InvalidOperationException("IRootObjectProvider");
			}
			var rootObject = rootProvider.RootObject as IResource;
			if (rootObject == null)
			{
				throw new InvalidOperationException("RootObject");
			}
			return rootObject.Manager.Load<Graphics.Resources.SystemFont>(this.Font);
		}
		#endregion

		#region Constructors
		public SystemFontExtension()
		{ }

		/// <summary>
		/// Inicjalizuje czcionkę za pomocą nazwy(w formacie: nazwa,rozmiar,[ib] - zobacz <see cref="Graphics.Resources.SystemFont"/>).
		/// </summary>
		/// <param name="font">Nazwa.</param>
		public SystemFontExtension(string font)
		{
			this.Font = font;
		}

		/// <summary>
		/// Inicjalizuje czcionkę.
		/// </summary>
		/// <param name="fontName">Nazwa czcionki.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="style">Style(i - italic, b - bold).</param>
		public SystemFontExtension(string fontName, int size, string style)
		{
			this.Font = string.Format("{0},{1},{2}", fontName, size, (style.Contains("i") ? "i" : "") + (style.Contains("b") ? "b" : ""));
		}

		/// <summary>
		/// Inicjalizuje czcionkę.
		/// </summary>
		/// <param name="fontName">Nazwa czcionki.</param>
		/// <param name="size">Rozmiar.</param>
		public SystemFontExtension(string fontName, int size)
		{
			this.Font = string.Format("{0},{1},", fontName, size);
		}
		#endregion
	}
}
