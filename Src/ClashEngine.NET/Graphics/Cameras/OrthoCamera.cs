using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Graphics.Cameras
{
	using EntitiesManager;
	using Interfaces.Graphics.Cameras;

	/// <summary>
	/// Kamera ortograficzna 2D jako encja.
	/// <see cref="Interfaces.Components.Cameras.InnerCamera"/>
	/// </summary>
	public class OrthoCamera
		: GameEntity, IOrthoCamera
	{
		#region IOrthoCamera Members
		/// <summary>
		/// Kamera.
		/// </summary>
		public Interfaces.Graphics.Components.IOrthoCamera Camera { get; private set; }
		#endregion

		/// <summary>
		/// Inicjalizuje kamerę.
		/// Zobacz: <see cref="Interfaces.Components.Cameras.IOrthoCamera"/>
		/// </summary>
		public OrthoCamera(RectangleF borders, Vector2 size, float speed, bool updateAlways, float zNear = 0.0f, float zFar = 1.0f)
			: base("OrthoCamera")
		{
			this.Camera = new Components.OrthoCamera(borders, size, speed, updateAlways, zNear, zFar);
		}

		public override void OnInit()
		{
			this.Components.Add(this.Camera);
		}
	}
}
