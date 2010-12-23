using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Resources
{
	/// <summary>
	/// Bazowy interfejs dla tekstur.
	/// </summary>
	public interface ITexture
		: IResource
	{
		/// <summary>
		/// Pobiera identyfikator(OpenGL) tekstury.
		/// </summary>
		int TextureId { get; }

		/// <summary>
		/// Rozmiar(w pikselach) tekstury.
		/// </summary>
		Vector2 Size { get; }

		/// <summary>
		/// Pobiera koordynaty tekstury.
		/// </summary>
		RectangleF Coordinates { get; }

		/// <summary>
		/// Ustawia teksturę jako aktualnie używaną.
		/// </summary>
		void Bind();
	}
}
