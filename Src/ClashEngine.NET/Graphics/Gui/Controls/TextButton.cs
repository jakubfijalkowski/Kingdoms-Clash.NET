using System.Drawing;
using OpenTK;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui.Controls;
	using Interfaces.Graphics.Objects;
	using Interfaces.Graphics.Resources;
	using Utilities;

	/// <summary>
	/// Przycisk z tekstem.
	/// </summary>
	public class TextButton
		: Base.RectangleGuiControlWithWasActive, ITextButton
	{
		#region Temp. conf.
		private static Vector2 ShadowOffset = new Vector2(3, 3);
		private static Vector2 ActiveOffset = new Vector2(2, 2);
		private static Vector2 TextOffset = new Vector2(2, 2);
		private static readonly Color Color = Color.Cyan;
		private static readonly Color TextColor = Color.Black;
		#endregion

		#region Private fields
		private ITexture TextTexture;
		private IQuad Background;
		private IQuad Shadow;
		private ISprite Text;
		#endregion

		#region ITextButton Members
		/// <summary>
		/// Tekst na przycisku.
		/// </summary>
		public string Label { get; private set; }

		/// <summary>
		/// Czcionka użyta do wyrenderowania tekstu.
		/// </summary>
		public IFont Font { get; private set; }

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
			if (this.Data.Active == this)
			{
				this.Background.Position += ActiveOffset;
				this.Text.Position += ActiveOffset;
			}

			this.Data.Renderer.Draw(this.Shadow);
			this.Data.Renderer.Draw(this.Background);
			this.Data.Renderer.Draw(this.Text);
			this.Data.Renderer.Flush();

			if (this.Data.Active == this)
			{
				this.Background.Position -= ActiveOffset;
				this.Text.Position -= ActiveOffset;
			}
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
		/// <param name="font">Czcionka użyta do wyrenderowania tekstu.</param>
		public TextButton(string id, RectangleF rectangle, string label, IFont font)
			: base(id, rectangle)
		{
			if (font == null)
			{
				throw new System.ArgumentNullException("font");
			}
			this.Label = label;
			this.TextTexture = font.DrawString(label, TextColor);
			this.Background = new Objects.Quad(rectangle.TopLeft(), rectangle.GetSize(), Color, 0.5f);
			this.Shadow = new Objects.Quad(rectangle.TopLeft() + ShadowOffset, rectangle.GetSize(), Color.DarkGray, 1);
			this.Text = new Objects.Sprite(this.TextTexture, rectangle.TopLeft() + TextOffset);
		}

		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="position">Pozycja.</param>
		/// <param name="size">Rozmiar.</param>
		/// <param name="label">Etykieta tekstowa.</param>
		/// <param name="font">Czcionka użyta do wyrenderowania tekstu.</param>
		public TextButton(string id, Vector2 position, Vector2 size, string label, IFont font)
			: this(id, new RectangleF(position.X, position.Y, size.X, size.Y), label, font)
		{ }
		#endregion
	}

	public static class ButtonExt
	{
		/// <summary>
		/// Sprawdza, czy wskazany przycisk został wciśnięty.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="id">Identyfikator.</param>
		/// <returns></returns>
		public static bool Button(this GuiContainer c, string id)
		{
			return c.Control(id) == 1;
		}
	}
}
