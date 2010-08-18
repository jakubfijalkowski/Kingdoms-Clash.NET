using System.Drawing;

namespace ClashEngine.NET.Interfaces.Resources
{
	/// <summary>
	/// Bazowy interfejs dla tekstur.
	/// </summary>
	public interface ITexture
		: ResourcesManager.IResource
	{
		/// <summary>
		/// Pobiera identyfikator(OpenGL) tekstury.
		/// </summary>
		int GetID { get; }

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
