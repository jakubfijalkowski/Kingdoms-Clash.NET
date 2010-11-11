using System;
using System.Drawing;

namespace ClashEngine.NET.Graphics.Resources
{
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Czcionka systemowa.
	/// Używa System.Drawing.Font.
	/// </summary>
	public class SystemFont
		: IFont
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("ClashEngine.NET");

		#region Private fields
		private Font Font;
		#endregion
		
		#region IFont Members
		/// <summary>
		/// Nazwa czcionki.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Rozmiar w pikselach.
		/// </summary>
		public int Size { get; private set; }

		/// <summary>
		/// Czy jest to czcionka pogrubiona.
		/// </summary>
		public bool Bold { get; private set; }

		/// <summary>
		/// Czy jest to czcionka pochylona.
		/// </summary>
		public bool Italic { get; private set; }

		/// <summary>
		/// Rysuje tekst do nowej tekstury.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Nowo utworzona tekstura.</returns>
		public ITexture DrawString(string text, System.Drawing.Color color)
		{
			ITexture tex = this.CreateEmptyText();
			this.DrawString(text, color, tex);
			return tex;
		}

		/// <summary>
		/// Rysuje tekst na istniejącą teksturę.
		/// </summary>
		/// <param name="text">Tekst do wypiania.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="onto">Tekstura. Musi być to tekstura zwrócona przez DrawString.</param>
		public void DrawString(string text, System.Drawing.Color color, ITexture onto)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tworzy pustą teksturę, by móc jej później użyć w metodzie <see cref="DrawString(text,ITexture)"/>.
		/// </summary>
		/// <returns>Nowa tekstura.</returns>
		public ITexture CreateEmptyText()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IResource Members
		/// <summary>
		/// Identyfikator czcionki - nazwa,rozmiar_w_pikselach[,ib]
		/// i - pochyła
		/// b - pogrubiona
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Nazwa czcionki - identycznie jak <see cref="Id"/>.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Manager-rodzic zasobu.
		/// </summary>
		public Interfaces.IResourcesManager Manager { get; set; }

		/// <summary>
		/// Wczytuje czcionkę parsując Id.
		/// </summary>
		/// <returns>Failure, gdy Id ma niepoprawny format, DefaultUsed, gdy nie znaleziono czcionki, inaczej Success.</returns>
		public Interfaces.ResourceLoadingState Load()
		{
			string[] values = this.Id.Split(',');
			int size = 0;
			if (values.Length != 3 || !int.TryParse(values[1].Trim(), out size))
			{
				return Interfaces.ResourceLoadingState.Failure;
			}
			this.Name = values[0].Trim();
			this.Size = size;
			this.Italic = values[2].Contains("i");
			this.Bold = values[2].Contains("b");

			this.Font = new Font(this.Name, this.Size,
				(this.Italic ? FontStyle.Italic : FontStyle.Regular) | (this.Bold ? FontStyle.Bold : FontStyle.Regular), GraphicsUnit.Pixel);
			if (this.Font.Name == "Microsoft Sans Serif")
			{
				return Interfaces.ResourceLoadingState.DefaultUsed;
			}

			return Interfaces.ResourceLoadingState.Success;
		}

		/// <summary>
		/// Zwalnia czcionkę.
		/// </summary>
		public void Free()
		{
			this.Font.Dispose();
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
