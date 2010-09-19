using System.Drawing;

namespace ClashEngine.NET.Interfaces.Components.Cameras
{
	using Interfaces.EntitiesManager;

	/// <summary>
	/// Kamera ortograficzna 2D.
	/// Buduje kamerę z czterech ograniczników: lewo, prawo, góra, dół. Domyślnie wyświetlany jest fragment 0..1/0..1
	/// i gracz może ją przesuwać za pomocą strzałek.
	/// TODO: dopisać przesuwanie za pomocą myszki.
	/// </summary>
	public interface IOrthoCamera
		: IComponent
	{
		/// <summary>
		/// Granice kamery.
		/// Right - Left nie może być większe od Width i
		/// Bottom - Top nie może być większe od Height.
		/// </summary>
		RectangleF Borders { get; }

		/// <summary>
		/// Rozmiar kamery.
		/// </summary>
		SizeF Size { get; }

		/// <summary>
		/// Aktualna pozycja(lewy górny róg).
		/// </summary>
		PointF CurrentPosition { get; }

		/// <summary>
		/// Szybkość poruszania kamery.
		/// </summary>
		float CameraSpeed { get; }

		/// <summary>
		/// Bliższa płaszczyzna kamery. Odpowiada parametrowi zNear GL.Ortho.
		/// </summary>
		float ZNear { get; }

		/// <summary>
		/// Dalsza płaszczyzna kamery. Odpowiada parametrowi zFar GL.Ortho.
		/// </summary>
		float ZFar { get; }

		/// <summary>
		/// Czy zawsze(co aktualizację komponentu, nie co przesunięcie) aktualizować macierz projekcji?
		/// </summary>
		bool UpdateAlways { get; }

		/// <summary>
		/// Przesuwa kamerę na wskazaną pozycję.
		/// Automatycznie koryguje pozycję jeśli kamera wychodzi poza granice.
		/// </summary>
		/// <param name="pt">Lewy górny róg kamery.</param>
		void MoveTo(PointF pt);

		/// <summary>
		/// Inicjalizuje kamerę.
		/// </summary>
		/// <param name="borders">Krawędzie kamery.
		/// Width nie może być większe od size.Width i
		/// Height nie może być większe od size.Height.
		/// </param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="speed">Szybkość poruszania się kamery.</param>
		/// <param name="updateAlways">Czy zawsze uaktualniać macierz projekcji?</param>
		/// <param name="zNear"><see cref="IOrthoCamera.ZNear"/></param>
		/// <param name="zFar"><see cref="IOrthoCamera.ZFar"/></param>
		void Init(RectangleF borders, SizeF size, float speed, bool updateAlways, float zNear = 0.0f, float zFar = 1.0f);
	}
}
