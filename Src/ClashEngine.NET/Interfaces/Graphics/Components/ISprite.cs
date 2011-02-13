using OpenTK;

namespace ClashEngine.NET.Interfaces.Graphics.Components
{
	using Graphics.Objects;

	/// <summary>
	/// Sprite - duszek - komponent prezentujący teksturę w miejscu encji gry.
	/// </summary>
	public interface ISprite
		: EntitiesManager.IRenderableComponent
	{
		/// <summary>
		/// Tekstura sprite'u.
		/// </summary>
		Resources.ITexture Texture { get; set; }

		/// <summary>
		/// Pozycja.
		/// </summary>
		Vector2 Position { get; set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		Vector2 Size { get; set; }

		/// <summary>
		/// Rotacja duszka.
		/// </summary>
		float Rotation { get; set; }

		/// <summary>
		/// Efekty duszka.
		/// </summary>
		SpriteEffect Effect { get; set; }

		/// <summary>
		/// Wymusza zachowanie proporcji duszka.
		/// </summary>
		bool MaintainAspectRatio { get; set; }
	}
}
