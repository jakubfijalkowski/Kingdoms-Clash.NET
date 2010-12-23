using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Obiekt renderera - tekst.
	/// </summary>
	public interface IText
		: IObject
	{
		/// <summary>
		/// Tekst, który został wpisany.
		/// </summary>
		string Content { get; }

		/// <summary>
		/// Pozycja obiektu.
		/// </summary>
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		Vector2 Size { get; set; }
	}
}
