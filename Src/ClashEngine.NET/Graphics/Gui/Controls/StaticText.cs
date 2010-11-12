using System.Drawing;

namespace ClashEngine.NET.Graphics.Gui.Controls
{
	using Interfaces.Graphics.Gui.Controls;
	using Interfaces.Graphics.Objects;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Kontrolka ze stałym tekstem(można go zmienić tylko programowo).
	/// </summary>
	public class StaticText
		: IStaticText
	{
		#region Private fields
		private string _Text = string.Empty;
		private Color _Color = Color.White;
		private ITexture TextTexture = null;
		private ISprite TextSprite = null;
		#endregion

		#region IStaticText Members
		/// <summary>
		/// Tekst kontrolki.
		/// </summary>
		public string Text
		{
			get { return this._Text; }
			set
			{
				this._Text = value;
				this.Font.DrawString(this._Text, this.Color, this.TextTexture);
			}
		}

		/// <summary>
		/// Kolor tekstu.
		/// </summary>
		public Color Color
		{
			get { return this._Color; }
			set
			{
				this._Color = value;
				this.Font.DrawString(this.Text, this.Color, this.TextTexture);
			}
		}

		/// <summary>
		/// Czcionka używana do renderowania tekstu.
		/// </summary>
		public IFont Font { get; private set; }

		/// <summary>
		/// Pozycja tekstu.
		/// </summary>
		public OpenTK.Vector2 Position
		{
			get { return this.TextSprite.Position; }
			set { this.TextSprite.Position = value; }
		}
		#endregion

		#region IGuiControl Members
		/// <summary>
		/// Identyfikator.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Nieużywane.
		/// </summary>
		public Interfaces.Graphics.Gui.IUIData Data { get; set; }

		/// <summary>
		/// Nie potrzebujemy zdarzeń - zawsze false.
		/// </summary>
		public bool PermanentActive
		{
			get { return false; }
		}

		/// <summary>
		/// Renderuje tekst.
		/// </summary>
		public void Render()
		{
			this.Data.Renderer.Draw(this.TextSprite);
		}

		/// <summary>
		/// Nie potrzebujemy jakichkolwiek zdarzeń - pomijamy.
		/// </summary>
		/// <returns></returns>
		public bool ContainsMouse()
		{
			return false;
		}

		/// <summary>
		/// Nieużywane.
		/// </summary>
		/// <param name="delta"></param>
		public void Update(double delta)
		{ }

		/// <summary>
		/// Nieużywane - zawsze zwraca 0.
		/// </summary>
		/// <returns></returns>
		public int Check()
		{
			return 0;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje kontrolkę.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="font">Czcionka.</param>
		/// <param name="text">Tekst.</param>
		/// <param name="color">Kolor.</param>
		/// <param name="position">Pozycja.</param>
		public StaticText(string id, IFont font, string text, Color color, OpenTK.Vector2 position)
		{
			if (font == null)
			{
				throw new System.ArgumentNullException("font");
			}
			this.Id = id;
			this.Font = font;
			this._Text = text;
			this._Color = color;

			this.TextTexture = this.Font.DrawString(text, color);
			this.TextSprite = new ClashEngine.NET.Graphics.Objects.Sprite(this.TextTexture, position);
		}
		#endregion
	}
}
