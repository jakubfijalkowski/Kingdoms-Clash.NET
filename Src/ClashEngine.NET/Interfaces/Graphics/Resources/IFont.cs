using System.Drawing;
using OpenTK;

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
		
		#region Drawing strings onto texture
		/// <summary>
		/// Rysuje tekst do nowej tekstury.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Nowo utworzona tekstura.</returns>
		ITexture DrawString(string text, Color color);

		/// <summary>
		/// Rysuje tekst do nowej tekstury.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Nowo utworzona tekstura.</returns>
		ITexture DrawString(string text, Vector4 color);

		/// <summary>
		/// Rysuje tekst na istniejącą teksturę.
		/// </summary>
		/// <param name="text">Tekst do wypiania.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="onto">Tekstura. Musi być to tekstura zwrócona przez DrawString.</param>
		void DrawString(string text, Color color, ITexture onto);

		/// <summary>
		/// Rysuje tekst na istniejącą teksturę.
		/// </summary>
		/// <param name="text">Tekst do wypiania.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="onto">Tekstura. Musi być to tekstura zwrócona przez DrawString.</param>
		void DrawString(string text, Vector4 color, ITexture onto);
		#endregion

		#region Drawing strings into objects
		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		Objects.IText Draw(string text, Color color);

		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		Objects.IText Draw(string text, Vector4 color);

		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		void Draw(string text, Color color, Objects.IText into);

		/// <summary>
		/// Rysuje tekst na obiekt renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		void Draw(string text, Vector4 color, Objects.IText into);
		#endregion
	}
}
