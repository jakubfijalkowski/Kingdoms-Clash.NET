using System.Drawing;
using OpenTK;
using System.ComponentModel;

namespace ClashEngine.NET.Interfaces.Graphics.Resources
{
	/// <summary>
	/// Czcionka.
	/// </summary>
	[TypeConverter(typeof(Converters.FontConverter))]
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

		#region Single line
		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		Objects.IText Draw(string text, Color color);

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		Objects.IText Draw(string text, Vector4 color);

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		void Draw(string text, Color color, Objects.IText into);

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		void Draw(string text, Vector4 color, Objects.IText into);
		#endregion

		#region Textbox
		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="textBox">Pole tekstowe, w którym ma się zmieścić tekst.</param>
		/// <returns>Utworzony obiekt.</returns>
		Objects.IText Draw(string text, Color color, RectangleF textBox);

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <returns>Utworzony obiekt.</returns>
		Objects.IText Draw(string text, Vector4 color, RectangleF textBox);

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="textBox">Pole tekstowe, w którym ma się zmieścić tekst.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		void Draw(string text, Color color, RectangleF textBox, Objects.IText into);

		/// <summary>
		/// Rysuje tekst do obiektu renderera.
		/// </summary>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="textBox">Pole tekstowe, w którym ma się zmieścić tekst.</param>
		/// <param name="into">Obiekt(utworzony wcześniej przez rodzica) w który zostanie wrysowany tekst.</param>
		void Draw(string text, Vector4 color, RectangleF textBox, Objects.IText into);
		#endregion
	}
}
