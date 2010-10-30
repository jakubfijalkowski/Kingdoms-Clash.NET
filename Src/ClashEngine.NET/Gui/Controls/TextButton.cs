using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Gui.Controls
{
	using Interfaces.Gui.Controls;

	/// <summary>
	/// Przycisk z tekstem.
	/// </summary>
	/// TODO: dopisać renderowanie tekstu.
	/// TODO: dopisać obsługę tematów.
	/// TODO: ogólnie poprawić renderowanie.
	public class TextButton
		: Base.RectangleGuiControlWithWasActive, ITextButton
	{
		private const float ShadowOffset = 3f;
		private const float ActiveOffset = 2f;
		private static readonly Color Color = Color.Cyan;

		#region ITextButton Members
		/// <summary>
		/// Tekst na przycisku.
		/// </summary>
		public string Label { get; private set; }

		/// <summary>
		/// Czy był wciśnięty.
		/// </summary>
		public bool Clicked { get; private set; }
		#endregion

		#region IGuiControl Members
		/// <summary>
		/// Nigdy nie może być "aktywny permanentnie".
		/// </summary>
		public override bool PermanentActive
		{
			get { return false; }
		}

		public override void Render()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Color3(Color.DarkGray);
			GL.Begin(BeginMode.Quads);
			GL.Vertex2(this.Rectangle.Left + ShadowOffset, this.Rectangle.Top + ShadowOffset);
			GL.Vertex2(this.Rectangle.Right + ShadowOffset, this.Rectangle.Top + ShadowOffset);
			GL.Vertex2(this.Rectangle.Right + ShadowOffset, this.Rectangle.Bottom + ShadowOffset);
			GL.Vertex2(this.Rectangle.Left + ShadowOffset, this.Rectangle.Bottom + ShadowOffset);
			GL.End();

			float offset = (this.Data.Active == this ? ActiveOffset : 0f);
			GL.Color3(Color);
			GL.Begin(BeginMode.Quads);
			GL.Vertex2(this.Rectangle.Left + offset, this.Rectangle.Top + offset);
			GL.Vertex2(this.Rectangle.Right + offset, this.Rectangle.Top + offset);
			GL.Vertex2(this.Rectangle.Right + offset, this.Rectangle.Bottom + offset);
			GL.Vertex2(this.Rectangle.Left + offset, this.Rectangle.Bottom + offset);
			GL.End();

			GL.Color3(Color.White);
		}

		/// <summary>
		/// Ustawia właściwość Clicked na true, gdy user puścił przycisk nad kontrolką.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			base.Update(delta);

			if (this.WasActive && this.Data.Active == null && this.Data.Hot == this)
			{
				this.Clicked = true;
			}
		}

		/// <summary>
		/// Sprawdza, czy przycisk był wciśnięty.
		/// </summary>
		/// <returns>1, gdy był, w przeciwnym razie 0.</returns>
		public override int Check()
		{
			if (this.Clicked)
			{
				this.Clicked = false;
				return 1;
			}
			return 0;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="rectangle">Prostokąt, w którym się znajduje kontrolka.</param>
		/// <param name="label">Tekstowa etykieta.</param>
		public TextButton(string id, RectangleF rectangle, string label)
			: base(id, rectangle)
		{
			this.Label = label;
		}

		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="label">Etykieta tekstowa.</param>
		public TextButton(string id, Vector2 position, Vector2 size, string label)
			: this(id, new RectangleF(position.X, position.Y, size.X, size.Y), label)
		{ }
		#endregion
	}
}
