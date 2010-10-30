using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Gui.Controls
{
	using Interfaces.Gui.Controls;

	/// <summary>
	/// Pole tekstowe.
	/// </summary>
	public class TextBox
		: Base.RectangleGuiControl, ITextBox
	{
		#region Private fields
		private static readonly Color Color = Color.Cyan;

		private Func<string> Get;
		private Action<string> Set;
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
			GL.BindTexture(TextureTarget.Texture2D, 0);

			GL.Color3(Color);
			GL.Begin(BeginMode.Quads);
			GL.Vertex2(this.Rectangle.Left, this.Rectangle.Top);
			GL.Vertex2(this.Rectangle.Right, this.Rectangle.Top);
			GL.Vertex2(this.Rectangle.Right, this.Rectangle.Bottom);
			GL.Vertex2(this.Rectangle.Left, this.Rectangle.Bottom);
			GL.End();

			GL.Color3(Color.White);
		}

		/// <summary>
		/// Uaktualnia wpisany tekst, gdy kontrolka jest aktywna.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			if (this.Data.Active == this && this.Data.Input.LastCharacter != '\0')
			{
				string newStr = this.Get();
				if (this.Data.Input.LastCharacter == '\b')
				{
					if (newStr.Length > 0)
					{
						newStr = newStr.Remove(newStr.Length - 1);
					}
				}
				else
				{
					newStr += this.Data.Input.LastCharacter;
				}
				this.Data.Input.LastCharacter = '\0';
				this.Set(newStr);
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
		public TextBox(string id, RectangleF rect, Func<string> get, Action<string> set)
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
		}

		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="position">Pozycja pola.</param>
		/// <param name="size">Rozmiar pola.</param>
		/// <param name="get">Metoda, która pobiera tekst wpisany do kontrolki.</param>
		/// <param name="set">Metoda, która zmienia tekst wpisany do kontrolki.</param>
		public TextBox(string id, Vector2 position, Vector2 size, Func<string> get, Action<string> set)
			: this(id, new RectangleF(position.X, position.Y, size.X, size.Y), get, set)
		{ }
		#endregion
	}
}
