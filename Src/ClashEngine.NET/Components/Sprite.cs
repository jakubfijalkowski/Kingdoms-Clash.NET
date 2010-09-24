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
	/// Vector2 Position - pozycja
	/// Vector2 Size - rozmiar
	/// float Rotation - rotacja
	/// </summary>
	/// <remarks>
	/// Rotacja nie jest zaimplementowana najwydajeniej - do zmiany.
	/// </remarks>
	public class Sprite
		: RenderableComponent, ISprite
	{
		#region Properties
		/// <summary>
		/// Pozycja duszka.
		/// </summary>
		public IAttribute<Vector2> Position { get; private set; }

		/// <summary>
		/// Rozmiar.
		/// </summary>
		public IAttribute<Vector2> Size { get; private set; }

		/// <summary>
		/// Rotacja.
		/// </summary>
		public IAttribute<float> Rotation { get; private set; }
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
		#endregion

		#region RenderableComponent members
		/// <summary>
		/// Pobiera wymagane atrybuty.
		/// </summary>
		/// <param name="owner"></param>
		public override void Init(IGameEntity owner)
		{
			base.Init(owner);

			this.Position = this.Owner.Attributes.GetOrCreate<Vector2>("Position");
			this.Size = this.Owner.Attributes.GetOrCreate<Vector2>("Size");
			this.Rotation = this.Owner.Attributes.GetOrCreate<float>("Rotation");
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
			var rotation = Matrix4.CreateRotationZ(this.Rotation.Value);
			var translation = Matrix4.CreateTranslation(new Vector3(this.Position.Value + (this.Size.Value / 2)));
			GL.MultMatrix(ref translation);
			GL.MultMatrix(ref rotation);


			GL.Begin(BeginMode.Quads);
			{
				GL.TexCoord2(this.TextureCoordinates.Left, this.TextureCoordinates.Top);
				//GL.Vertex2(this.Position.Value);
				GL.Vertex2(-this.Size.Value / 2);

				GL.TexCoord2(this.TextureCoordinates.Right, this.TextureCoordinates.Top);
				//GL.Vertex2(this.Position.Value.X + this.Size.Value.X, this.Position.Value.Y);
				GL.Vertex2(this.Size.Value.X / 2, -this.Size.Value.Y / 2);

				GL.TexCoord2(this.TextureCoordinates.Right, this.TextureCoordinates.Bottom);
				//GL.Vertex2(this.Position.Value.X + this.Size.Value.X, this.Position.Value.Y + this.Size.Value.Y);
				GL.Vertex2(this.Size.Value / 2);

				GL.TexCoord2(this.TextureCoordinates.Left, this.TextureCoordinates.Bottom);
				//GL.Vertex2(this.Position.Value.X, this.Position.Value.Y + this.Size.Value.Y);
				GL.Vertex2(-this.Size.Value.X / 2, this.Size.Value.Y / 2);
			}
			GL.End();
			GL.PopMatrix();
		}
		#endregion
	}
}
