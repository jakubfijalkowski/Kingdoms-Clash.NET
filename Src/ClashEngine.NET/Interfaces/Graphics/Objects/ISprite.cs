using System;
using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Objects
{
	/// <summary>
	/// Efekty dla duszków.
	/// </summary>
	[Flags]
	public enum SpriteEffect
	{
		/// <summary>
		/// Bez efektu.
		/// </summary>
		No = 0x0,

		/// <summary>
		/// Odbija teksturę duszka w poziomie.
		/// </summary>
		FlipHorizontally = 0x1,

		/// <summary>
		/// Odbija teksturę duszka w pionie.
		/// </summary>
		FlipVertically = 0x2
	}

	/// <summary>
	/// Obiekt renderera - duszek.
	/// </summary>
	public interface ISprite
		: IObject
	{
		/// <summary>
		/// Pozycja.
		/// </summary>
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		Vector2 Size { get; set; }

		/// <summary>
		/// Efekty.
		/// </summary>
		SpriteEffect Effect { get; set; }
	}
}
