namespace ClashEngine.NET.Interfaces.Graphics
{
	/// <summary>
	/// Bazowy interfejs dla kamer.
	/// </summary>
	public interface ICamera
	{
		/// <summary>
		/// Rozmiar viewportu po przekształceniach kamery.
		/// </summary>
		OpenTK.Vector2 Size { get; }

		/// <summary>
		/// Minimalna głębokość widoczna w kamerze.
		/// </summary>
		float ZNear { get; }

		/// <summary>
		/// Maksymalna głębokość widoczna w kamerze.
		/// </summary>
		float ZFar { get; }

		/// <summary>
		/// Czy macierze wymagają aktualizacji.
		/// Używająca klasa zewnętrzna musi zapewnić, że przed zmianą NeedUpdate na false zostaną pobrane ViewMatrix i ProjectionMatrix.
		/// </summary>
		bool NeedUpdate { get; set; }

		/// <summary>
		/// Pobiera macierz widoku kamery.
		/// </summary>
		OpenTK.Matrix4 ViewMatrix { get; }

		/// <summary>
		/// Pobiera macierz projekcji kamery.
		/// </summary>
		OpenTK.Matrix4 ProjectionMatrix { get; }
	}
}
