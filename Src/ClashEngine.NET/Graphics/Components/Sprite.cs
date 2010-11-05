using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ClashEngine.NET.Components
{
	using EntitiesManager;
	using Interfaces.Components;
	using Interfaces.EntitiesManager;
	using Interfaces.Resources;
	
	/// <summary>
	/// Sprite - duszek - wyświetla teksturę w miejscu encji.
	/// 
	/// Wymagane atrybuty komponentu:
	/// Vector2 Position - pozycja(lewy górny róg)
	/// Vector2 Size - rozmiar
	/// float Rotation - rotacja(w radianach)
	/// </summary>
	public class Sprite
		: RenderableComponent, ISprite
	{
		private IAttribute<Vector2> Position_;
		private IAttribute<Vector2> Size_;
		private IAttribute<float> Rotation_;

		#region Properties
		/// <summary>
		/// Pozycja(lewy górny róg) duszka.
		/// </summary>
		public Vector2 Position
		{
			get { return this.Position_.Value; }
			set { this.Position_.Value = value; }
		}

		/// <summary>
		/// Rozmiar.
		/// </summary>
		public Vector2 Size
		{
			get { return this.Size_.Value; }
			set { this.Size_.Value = value; }
		}

		/// <summary>
		/// Rotacja.
		/// </summary>
		public float Rotation
		{
			get { return this.Rotation_.Value; }
			set { this.Rotation_.Value = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje nowego duszka domyslnymi wartościami - jest niezdatny do użytku.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		public Sprite(string id)
			: base(id)
		{
			this.Texture = null;
			this.TextureCoordinates = RectangleF.Empty;
		}

		/// <summary>
		/// Inicjalizuje nowego duszka.
		/// </summary>
		/// <param name="id">Identyfikator.</param>
		/// <param name="texture">Tekstura która zostanie pokryty.</param>
		/// <exception cref="ArgumentNullException">Nie podano tekstury.</exception>
		public Sprite(string id, ITexture texture)
			: base(id)
		{
			this.Init(texture);
		}

		/// <summary>
		/// Inicjalizuje duszka daną teksturą.
		/// Jeśli duszek był już zainicjalizowany, nadpisuje poprzednie ustawienia(ale nie zmienia atrybutów!).
		/// </summary>
		/// <param name="texture">Tekstura która zostanie pokryty.</param>
		/// <exception cref="ArgumentNullException">Nie podano tekstury.</exception>
		public void Init(ITexture texture)
		{
			if (texture == null)
			{
				throw new ArgumentNullException();
			}
			this.Texture = texture;
			this.TextureCoordinates = texture.Coordinates;
		}
		#endregion

		#region ISprite members
		/// <summary>
		/// Tekstura sprite'u.
		/// </summary>
		public ITexture Texture { get; private set; }

		/// <summary>
		/// Koordynaty tekstury. Domyślnie tekstura pokrywa cały sprite.
		/// </summary>
		public RectangleF TextureCoordinates { get; set; }

		/// <summary>
		/// Odbija teskturę sprite'a w poziomie.
		/// </summary>
		public void FlipHorizontal()
		{
			this.TextureCoordinates = RectangleF.FromLTRB(this.TextureCoordinates.Right, this.TextureCoordinates.Top,
				this.TextureCoordinates.Left, this.TextureCoordinates.Bottom);
		}

		/// <summary>
		/// Odbija teskturę sprite'a w pionie.
		/// </summary>
		public void FlipVertical()
		{
			this.TextureCoordinates = RectangleF.FromLTRB(this.TextureCoordinates.Left, this.TextureCoordinates.Bottom,
				this.TextureCoordinates.Right, this.TextureCoordinates.Top);
		}
		#endregion

		#region RenderableComponent members
		/// <summary>
		/// Pobiera wymagane atrybuty.
		/// </summary>
		/// <param name="owner"></param>
		public override void OnInit()
		{
			this.Position_ = this.Owner.Attributes.GetOrCreate<Vector2>("Position");
			this.Size_ = this.Owner.Attributes.GetOrCreate<Vector2>("Size");
			this.Rotation_ = this.Owner.Attributes.GetOrCreate<float>("Rotation");
		}

		/// <summary>
		/// Nieużywana.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			//Nie potrzebujemy aktualizacji.
		}

		/// <summary>
		/// Rysuje duszka.
		/// W tej wersji wykorzystywana jest najgorsza, ale i najprostsza, możlwość rysowania - GL.Begin i GL.End.
		/// </summary>
		/// <remarks>
		///		Rysowanie odbywa się zgodnie z ruchem wskazówek zegara począwszy od lewego, górnego wierzchołka.
		/// </remarks>
		public override void Render()
		{
			GL.LoadIdentity();
			this.Texture.Bind();

			GL.PushMatrix();
			//var rotation = Matrix4.CreateRotationZ(this.Rotation);
			//var translation = Matrix4.CreateTranslation(new Vector3(this.Position + (this.Size / 2)));
			//GL.MultMatrix(ref translation);
			//GL.MultMatrix(ref rotation);

			GL.Translate(new Vector3(this.Position + (this.Size / 2)));
			GL.Rotate(MathHelper.RadiansToDegrees(this.Rotation), 0f, 0f, 1f);
			
			GL.Begin(BeginMode.Quads);
			{
				GL.TexCoord2(this.TextureCoordinates.Left, this.TextureCoordinates.Top);
				//GL.Vertex2(this.Position.Value);
				GL.Vertex2(-this.Size / 2);

				GL.TexCoord2(this.TextureCoordinates.Right, this.TextureCoordinates.Top);
				//GL.Vertex2(this.Position.Value.X + this.Size.Value.X, this.Position.Value.Y);
				GL.Vertex2(this.Size.X / 2, -this.Size.Y / 2);

				GL.TexCoord2(this.TextureCoordinates.Right, this.TextureCoordinates.Bottom);
				//GL.Vertex2(this.Position.Value.X + this.Size.Value.X, this.Position.Value.Y + this.Size.Value.Y);
				GL.Vertex2(this.Size / 2);

				GL.TexCoord2(this.TextureCoordinates.Left, this.TextureCoordinates.Bottom);
				//GL.Vertex2(this.Position.Value.X, this.Position.Value.Y + this.Size.Value.Y);
				GL.Vertex2(-this.Size.X / 2, this.Size.Y / 2);
			}
			GL.End();
			GL.PopMatrix();
		}
		#endregion
	}
}
