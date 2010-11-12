using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Gui.Controls
{
	using Resources;

	/// <summary>
	/// Kontrolka ze stałym tekstem(można go zmienić tylko programowo).
	/// </summary>
	public interface IStaticText
		: IGuiControl
	{
		/// <summary>
		/// Tekst kontrolki.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		/// Kolor tekstu.
		/// </summary>
		System.Drawing.Color Color { get; set; }

		/// <summary>
		/// Czcionka używana do renderowania tekstu.
		/// </summary>
		IFont Font { get; }

		/// <summary>
		/// Pozycja tekstu.
		/// </summary>
		Vector2 Position { get; set; }
	}
}
