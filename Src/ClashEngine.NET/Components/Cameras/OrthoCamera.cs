using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Components.Cameras
{
	using EntitiesManager;
	using Interfaces.Components.Cameras;

	public class OrthoCamera
		: Component, IOrthoCamera
	{
		#region IOrthoCamera Properties
		/// <summary>
		/// Granice kamery.
		/// </summary>
		public RectangleF Borders { get; private set; }

		/// <summary>
		/// Rozmiar kamery.
		/// </summary>
		public SizeF Size { get; private set; }

		/// <summary>
		/// Aktualna pozycja(lewy górny róg).
		/// </summary>
		public PointF CurrentPosition { get; private set; }

		/// <summary>
		/// Szybkość poruszania kamery.
		/// </summary>
		public float CameraSpeed { get; private set; }

		/// <summary>
		/// Bliższa płaszczyzna kamery. Odpowiada parametrowi zNear GL.Ortho.
		/// </summary>
		public float ZNear { get; private set; }

		/// <summary>
		/// Dalsza płaszczyzna kamery. Odpowiada parametrowi zFar GL.Ortho.
		/// </summary>
		public float ZFar { get; private set; }
		#endregion

		/// <summary>
		/// Inicjalizuje kamerę.
		/// Domyślnie ustawiana jest w lewym górnym rogu granic.
		/// </summary>
		/// <param name="borders"> Krawędzie kamery.
		/// Width nie może być większe od size.Width i
		/// Height nie może być większe od size.Height.
		/// </param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="speed">Szybkość poruszania się kamery.</param>
		/// <param name="zNear"><see cref="OrthoCamera.ZNear"/></param>
		/// <param name="zFar"><see cref="OrthoCamera.ZFar"/></param>
		/// <exception cref="ArgumentException">Rozmiar jest większy od granic ekranu.</exception>
		public OrthoCamera(RectangleF borders, SizeF size, float speed, float zNear = 0.0f, float zFar = 1.0f)
			: base("OrthoCamera")
		{
			if (size.Width > borders.Width || size.Height > borders.Height)
			{
				throw new ArgumentException("Size is greater than borders", "size");
			}
			this.Borders = borders;
			this.Size = size;
			this.CameraSpeed = speed;
			this.ZNear = zNear;
			this.ZFar = zFar;
			this.CurrentPosition = new PointF(borders.Left, borders.Top);

			this.UpdateMatrix();
		}

		/// <summary>
		/// Aktualizuje położenie kamery jeśli któryś z przycisków jest wciśnięty.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			PointF pt = this.CurrentPosition;
			if (Input.Instance.Keyboard[OpenTK.Input.Key.Left])
			{
				pt.X -= (float)(delta * this.CameraSpeed);
			}
			if (Input.Instance.Keyboard[OpenTK.Input.Key.Right])
			{
				pt.X += (float)(delta * this.CameraSpeed);
			}
			if (Input.Instance.Keyboard[OpenTK.Input.Key.Up])
			{
				pt.Y -= (float)(delta * this.CameraSpeed);
			}
			if (Input.Instance.Keyboard[OpenTK.Input.Key.Down])
			{
				pt.Y += (float)(delta * this.CameraSpeed);
			}
			this.MoveTo(pt);
		}

		/// <summary>
		/// Przesuwa kamerę na wskazaną pozycję.
		/// Jeśli pozycja jest poza zakresem automatycznie ją koryguje.
		/// </summary>
		/// <param name="pt">Lewy górny róg ekranu.</param>
		public void MoveTo(PointF pt)
		{
			if (pt.X < this.Borders.Left)
			{
				pt.X = this.Borders.Left;
			}
			else if (pt.X + this.Size.Width > this.Borders.Right)
			{
				pt.X = this.Borders.Right - this.Size.Width;
			}

			if (pt.Y < this.Borders.Top)
			{
				pt.Y = this.Borders.Top;
			}
			else if (pt.Y + this.Size.Height > this.Borders.Bottom)
			{
				pt.Y = this.Borders.Bottom - this.Size.Height;
			}

			//Uaktualniamy tylko jeśli pozycja się różni.
			if (this.CurrentPosition != pt)
			{
				this.CurrentPosition = pt;
				this.UpdateMatrix();
			}
		}

		/// <summary>
		/// Uaktualnia macież projekcji.
		/// </summary>
		private void UpdateMatrix()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(this.CurrentPosition.X, this.CurrentPosition.X + this.Size.Width, this.CurrentPosition.Y + this.Size.Height, this.CurrentPosition.Y, this.ZNear, this.ZFar);

			GL.MatrixMode(MatrixMode.Modelview);
		}
	}
}
