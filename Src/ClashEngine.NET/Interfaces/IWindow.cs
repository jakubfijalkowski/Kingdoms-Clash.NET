using System;
using OpenTK;
using OpenTK.Graphics;

namespace ClashEngine.NET.Interfaces
{
	/// <summary>
	/// Flagi okna.
	/// </summary>
	/// <remarks>
	/// Fullscreen odpowiada ustawieniu <see cref="OpenTK.INativeWindow.WindowState"/> na <see cref="OpenTK.WindowState.Fullscreen"/>.
	/// FixedSize odpowiada ustawieniu <see cref="OpenTK.INativeWindow.WindowBorder"/> na <see cref="OpenTK.WindowBorder.Fixed"/>.
	/// VSync odpowiada ustawieniu <see cref="OpenTK.Graphics.IGraphicsContext.VSync"/> na true.
	/// Visible odpowiada ustawieniu <see cref="OpenTK.INativeWindow.Visible"/> na true.
	/// </remarks>
	[Flags]
	public enum WindowFlags
	{
		/// <summary>
		/// Brak
		/// </summary>
		None = 0x00,

		/// <summary>
		/// Pełny ekran.
		/// </summary>
		Fullscreen = 0x01,

		/// <summary>
		/// Stały rozmiar.
		/// </summary>
		FixedSize = 0x02,

		/// <summary>
		/// Synchronizacja pionowa.
		/// </summary>
		VSync = 0x04,

		/// <summary>
		/// Okno jest widoczne.
		/// </summary>
		Visible = 0x08,

		/// <summary>
		/// Czy da się okno zamknąć za pomocą kombinacji Alt+F4.
		/// </summary>
		AltF4 = 0x10,

		/// <summary>
		/// Domyślne flagi.
		/// FixedSize, VSync, Visible i, jeśli tryb Debug, AltF4
		/// </summary>
		Default = FixedSize | VSync | Visible
#if DEBUG
			| AltF4
#endif
	}

	/// <summary>
	/// Okno.
	/// </summary>
	public interface IWindow
		: INativeWindow
	{
		/// <summary>
		/// Kontekst grafiki.
		/// </summary>
		IGraphicsContext Context { get; }

		/// <summary>
		/// Rozmiar okna.
		/// </summary>
		new Vector2 Size { get; set; }

		/// <summary>
		/// Flagi okna.
		/// Część można używać zamiennie z właściwościami. Patrz sekcje "Remarks" <see cref="WindowFlags"/>.
		/// </summary>
		WindowFlags Flags { get; set; }

		/// <summary>
		/// Czy kontekst graficzny okna jest aktualnie "wybrany".
		/// </summary>
		bool IsCurrent { get; }

		/// <summary>
		/// Wejście okna.
		/// </summary>
		IInput Input { get; }

		/// <summary>
		/// Czy okno jest akutalnie zamykane.
		/// </summary>
		bool IsClosing { get; }

		/// <summary>
		/// Używana wersja OpenGL.
		/// </summary>
		Version OpenGLVersion { get; }

		/// <summary>
		/// Używa kontekstu tego okna jako aktualnego.
		/// </summary>
		void Use();

		/// <summary>
		/// Wyświetla zawartość okna.
		/// </summary>
		/// <remarks>
		/// Wybiera okno(jeśli nie jest wybrane) i wywołuje <see cref="OpenTK.Graphics.IGraphicsContext.SwapBuffers"/>.
		/// </remarks>
		void Show();
	}
}
