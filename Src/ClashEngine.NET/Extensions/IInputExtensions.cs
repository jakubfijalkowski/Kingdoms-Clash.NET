using OpenTK;

namespace ClashEngine.NET.Extensions
{
	using Interfaces;
	using Interfaces.Graphics;
	using Interfaces.Graphics.Cameras;

	/// <summary>
	/// Rozszerzenia dla IInput.
	/// </summary>
	public static class IInputExtensions
	{
		/// <summary>
		/// Transformuje pozycje myszki do koordynatów kamery.
		/// </summary>
		/// <param name="input">this</param>
		/// <param name="camera">Kamera.</param>
		/// <returns></returns>
		public static Vector2 Transform(this IInput input, ICamera camera)
		{
			var transformedPos = new Vector2(
				input.MousePosition.X / input.Owner.Size.X * camera.Size.X,
				input.MousePosition.Y / input.Owner.Size.Y * camera.Size.Y);
			
			if (camera is IMovable2DCamera)
			{
				transformedPos.X += (camera as IMovable2DCamera).CurrentPosition.X;
				transformedPos.Y += (camera as IMovable2DCamera).CurrentPosition.Y;
			}
			return transformedPos;
		}
	}
}
