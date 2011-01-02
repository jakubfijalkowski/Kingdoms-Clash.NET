using System;
using OpenTK;

namespace ClashEngine.NET.Graphics.Components
{
	using EntitiesManager;
	using Interfaces.EntitiesManager;
	using Interfaces.Graphics.Components;
	using Interfaces.Graphics.Resources;

	/// <summary>
	/// Sprite - duszek - wyświetla teksturę w miejscu encji.
	/// 
	/// Wymagane atrybuty komponentu:
	/// Vector2 Position - pozycja(lewy górny róg)
	/// Vector2 Size - rozmiar
	/// </summary>
	public class Sprite
		: RenderableComponent, ISprite
	{
		#region Private fields
		private IAttribute<Vector2> _Position;
		private IAttribute<Vector2> _Size;
		private Objects.Sprite _Sprite;
		private ITexture _Texture;
		#endregion

		#region ISprite Members
		/// <summary>
		/// Tekstura.
		/// Zmiana tekstury po zainicjowaniu komponentu nie będzie oddziaływać na obiekt.
		/// </summary>
		public ITexture Texture
		{
			get { return this._Sprite.Texture; }
			set { this._Texture = value; }
		}

		/// <summary>
		/// Pozycja(lewy górny róg) duszka.
		/// </summary>
		public Vector2 Position
		{
			get { return this._Position.Value; }
			set { this._Position.Value = value; }
		}

		/// <summary>
		/// Rozmiar.
		/// </summary>
		public Vector2 Size
		{
			get { return this._Size.Value; }
			set { this._Size.Value = value; }
		}

		/// <summary>
		/// Efekty.
		/// </summary>
		public Interfaces.Graphics.Objects.SpriteEffect Effect
		{
			get { return this._Sprite.Effect; }
			set { this._Sprite.Effect = value; }
		}

		/// <summary>
		/// Wymusza zachowanie proporcji duszka.
		/// </summary>
		public bool MaintainAspectRatio
		{
			get { return this._Sprite.MaintainAspectRatio; }
			set { this._Sprite.MaintainAspectRatio = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowego duszka.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public Sprite(string id)
			: base(id)
		{ }

		/// <summary>
		/// Inicjalizuje nowego duszka.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="texture">Tekstura która zostanie pokryty.</param>
		/// <exception cref="ArgumentNullException">Nie podano tekstury.</exception>
		public Sprite(string id, ITexture texture)
			: base(id)
		{
			if (texture == null)
			{
				throw new ArgumentNullException("texture");
			}
			this._Texture = texture;
		}
		#endregion

		#region RenderableComponent members
		/// <summary>
		/// Pobiera wymagane atrybuty.
		/// </summary>
		/// <param name="owner"></param>
		public override void OnInit()
		{
			this._Position = this.Owner.Attributes.GetOrCreate<Vector2>("Position");
			this._Position.PropertyChanged += (a, b) => this._Sprite.Position = this.Position;

			this._Size = this.Owner.Attributes.GetOrCreate<Vector2>("Size");
			this._Size.PropertyChanged += (a, b) =>
				{
					this._Sprite.Size = this.Size;
					if (this._Sprite.Size != this.Size) //Nadzorujemy aspect ratio
					{
						this.Size = this._Sprite.Size;
					}
				};

			this._Sprite = new Objects.Sprite(this._Texture, this.Position, this.Size);
		}

		/// <summary>
		/// Nieużywana.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{ }

		/// <summary>
		/// Rysuje duszka.
		/// </summary>
		public override void Render()
		{
			if (this.Position != this._Sprite.Position)
			{
				this._Sprite.Position = this.Position;
			}
			this.Renderer.Draw(this._Sprite);
		}
		#endregion
	}
}
