using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Obiekt renderera - duszek.
	/// </summary>
	public interface ISprite
		: IObject
	{
		/// <summary>
		/// Pozycja duszka.
		/// </summary>
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar duszka.
		/// </summary>
		Vector2 Size { get; set; }
	}
}
