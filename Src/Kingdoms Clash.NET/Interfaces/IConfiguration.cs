namespace Kingdoms_Clash.NET.Interfaces
{
	/// <summary>
	/// Konfiguracja gry.
	/// Powinna być jedna na grę i dostępna z każdego miejsca.
	/// </summary>
	public interface IConfiguration
	{
		/// <summary>
		/// Szerokość okna.
		/// </summary>
		int WindowWidth { get; }

		/// <summary>
		/// Wysokość okna.
		/// </summary>
		int WindowHeight { get; }

		/// <summary>
		/// Czy okno ma być pełnoekranowe.
		/// </summary>
		bool Fullscreen { get; }

		/// <summary>
		/// Określa ile pikseli przypada na jedną jednostkę gry.
		/// Obliczane na podstawie aktualnej rozdzielczości.
		/// </summary>
		float PixelsWidthPerUnit { get; }

		/// <summary>
		/// Określa ile pikseli przypada na jedną jednostkę gry.
		/// Obliczane na podstawie aktualnej rozdzielczości.
		/// </summary>
		float PixelsHeightPerUnit { get; }
	}
}
