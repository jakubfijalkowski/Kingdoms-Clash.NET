using System.Drawing;

namespace ClashEngine.NET.Interfaces.Graphics.Resources
{
	/// <summary>
	/// Czcionka.
	/// </summary>
	public interface IFont
		: IResource
	{
		#region Properties
		/// <summary>
		/// Nazwa czcionki.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Rozmiar w pikselach.
		/// </summary>
		int Size { get; }

		/// <summary>
		/// Czy jest to czcionka pogrubiona.
		/// </summary>
		bool Bold { get; }

		/// <summary>
		/// Czy jest to czcionka pochylona.
		/// </summary>
		bool Italic { get; }
		#endregion
		
		#region Methods
		/// <summary>
		/// Rysuje tekst do nowej tekstury.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Nowo utworzona tekstura.</returns>
		ITexture DrawString(string text, Color color);

		/// <summary>
		/// Rysuje tekst na istniejącą teksturę.
		/// </summary>
		/// <param name="text">Tekst do wypiania.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="onto">Tekstura. Musi być to tekstura zwrócona przez DrawString.</param>
		void DrawString(string text, Color color, ITexture onto);

		/// <summary>
		/// Tworzy pustą teksturę, by móc jej później użyć w metodzie <see cref="DrawString(text,ITexture)"/>.
		/// </summary>
		/// <returns>Nowa tekstura.</returns>
		ITexture CreateEmptyText();
		#endregion
	}
}
