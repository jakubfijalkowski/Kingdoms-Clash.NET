using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui.Controls;
	using Interfaces.Graphics.Objects;
	using Interfaces.Graphics.Resources;
	using Utilities;

	/// <summary>
	/// Pole tekstowe.
	/// </summary>
	public class TextBox
		: Base.RectangleGuiControl, ITextBox
	{
		#region Private fields
		private static readonly Color Color = Color.Cyan;
		private static readonly Color TextColor = Color.Black;
		private static readonly Vector2 TextOffset = new Vector2(2, 2);

		private Func<string> Get;
		private Action<string> Set;

		private IFont Font;
		private ITexture TextTexture;
		private IQuad Background;
		private ISprite Text;
		#endregion

		#region IGuiControl Members
		/// <summary>
		/// Musimy trzymać "aktywność".
		/// </summary>
		public override bool PermanentActive
		{
			get { return true; }
		}

		public override void Render()
		{
			this.Data.Renderer.Draw(this.Background);
			this.Data.Renderer.Draw(this.Text);
		}

		/// <summary>
		/// Uaktualnia wpisany tekst, gdy kontrolka jest aktywna.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			if (this.Data.Active == this && this.Data.Input.LastCharacter != '\0')
			{
				bool changed = false;
				string newStr = this.Get();
				if (this.Data.Input.LastCharacter == '\b')
				{
					if (newStr.Length > 0)
					{
						newStr = newStr.Remove(newStr.Length - 1);
						changed = true;
					}
				}
				else
				{
					newStr += this.Data.Input.LastCharacter;
					changed = true;
				}
				if (changed)
				{
					this.Data.Input.LastCharacter = '\0';
					this.Set(newStr);
					this.Font.DrawString(newStr, TextColor, this.TextTexture);
					this.Text = new Objects.Sprite(this.TextTexture, this.Rectangle.TopLeft() + TextOffset);
				}
			}
		}

		/// <summary>
		/// Sprawdza, czy kontrolka jest aktywna.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			return (this.Data.Active == this ? 1 : 0);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="rect">Prostokąt z polem tekstowym.</param>
		/// <param name="get">Metoda, która pobiera tekst wpisany do kontrolki.</param>
		/// <param name="set">Metoda, która zmienia tekst wpisany do kontrolki.</param>
		/// <param name="font">Czcionka tekstu.</param>
		public TextBox(string id, RectangleF rect, Func<string> get, Action<string> set, IFont font)
			: base(id, rect)
		{
			if (get == null)
			{
				throw new ArgumentNullException("get");
			}
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}
			this.Get = get;
			this.Set = set;
			this.Font = font;

			this.TextTexture = this.Font.DrawString(this.Get(), TextColor);
			this.Background = new Objects.Quad(rect.TopLeft(), rect.GetSize(), Color, 1);
			this.Text = new Objects.Sprite(this.TextTexture, rect.TopLeft() + TextOffset);
		}

		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="position">Pozycja pola.</param>
		/// <param name="size">Rozmiar pola.</param>
		/// <param name="get">Metoda, która pobiera tekst wpisany do kontrolki.</param>
		/// <param name="set">Metoda, która zmienia tekst wpisany do kontrolki.</param>
		/// <param name="font">Czcionka tekstu.</param>
		public TextBox(string id, Vector2 position, Vector2 size, Func<string> get, Action<string> set, IFont font)
			: this(id, new RectangleF(position.X, position.Y, size.X, size.Y), get, set, font)
		{ }
		#endregion
	}
}
