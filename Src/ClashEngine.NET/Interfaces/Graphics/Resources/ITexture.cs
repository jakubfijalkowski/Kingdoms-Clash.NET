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
		/// Dane wpisane przez użytkownika.
		/// Powinny być pobierane z(w takiej kolejności):
		///		PropertyTagImageTitle
		///		PropertyTagImageDescription
		///		PropertyTagExifUserComment
		///		PropertyTagSoftwareUsed
		///	Jeśli nie istnieją - powinny być pobrane z nazwy pliku(pomiędzy [ i ], włącznie z [ i ]);
		/// </summary>
		string UserData { get; }

		/// <summary>
		/// Ustawia teksturę jako aktualnie używaną.
		/// </summary>
		void Bind();
	}
}
