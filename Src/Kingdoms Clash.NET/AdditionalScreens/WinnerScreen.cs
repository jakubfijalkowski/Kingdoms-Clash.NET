using ClashEngine.NET;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Graphics.Objects;
using ClashEngine.NET.Graphics.Resources;
using OpenTK;

namespace Kingdoms_Clash.NET.AdditionalScreens
{
	/// <summary>
	/// Ekran "wygranej" gracza.
	/// Id = WinnerScreen.
	/// </summary>
	public class WinnerScreen
		: Screen
	{
		#region Private Fields
		/// <summary>
		/// Rozmiar obrazka z napisem.
		/// </summary>
		private static readonly Vector2 ImageSize = new Vector2(426, 29);

		/// <summary>
		/// Obrazek.
		/// </summary>
		private WinnerImage Image = new WinnerImage();

		/// <summary>
		/// Stan gry.
		/// </summary>
		private Interfaces.IGameState GameState;
		#endregion

		#region Constructors
		public WinnerScreen(Interfaces.IGameState gameState)
			: base("WinnerScreen", ClashEngine.NET.Interfaces.ScreenType.Normal)
		{
			this.GameState = gameState;
		}
		#endregion

		#region Screen Members
		public override void OnInit()
		{
			base.OnInit();

			float aspect = ImageSize.X / ImageSize.Y;
			this.Entities.Add(new ClashEngine.NET.Graphics.Cameras.OrthoCamera(new System.Drawing.RectangleF(0f, 0f, 1f, 1f * aspect), new OpenTK.Vector2(1f, 1f * aspect), 0f, true));
			this.Entities.Add(this.Image);
		}

		public override void Update(double delta)
		{
			if (this.HandleInput())
			{
				return;
			}
			base.Update(delta);
		}

		#region Events
		/// <summary>
		/// Po naciśnięciu R resetuje grę.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private bool HandleInput()
		{
			if (this.Input[OpenTK.Input.Key.R])
			{
				this.GameState.Reset();
				this.Deactivate();
				return true;
			}
			return false;
		}
		#endregion
		#endregion

		/// <summary>
		/// Zmienia wygranego gracza.
		/// </summary>
		/// <param name="b">Jeśli true wygrał gracz B, inaczej wygrał gracz A.</param>
		public void ChangeWinner(bool b)
		{
			this.Image.ChangeWinner(b);
		}

		#region Private entities
		/// <summary>
		/// Obrazek ze zwycięzcą.
		/// </summary>
		private class WinnerImage
			: GameEntity
		{
			#region Private Fields
			private Sprite ImageAWon;
			private Sprite ImageBWon;
			private Sprite Current;
			#endregion

			#region Constructors
			public WinnerImage()
				: base("Player \"A\" has won the match!")
			{ }
			#endregion

			#region Screen Members
			public override void OnInit()
			{
				float aspect = (ImageSize.X / ImageSize.Y);
				float w = 0.8f;
				float h = aspect / 12f;
				var size = new Vector2(w, h);
				var position = new Vector2((1f - w) / 2f, (1f * aspect - h) / 2f);

				this.ImageAWon = new Sprite(this.Content.Load<Texture>("PlayerAWon.png"), position, size);
				this.ImageBWon = new Sprite(this.Content.Load<Texture>("PlayerBWon.png"), position, size);
				this.Current = this.ImageAWon;
			}

			public override void Render()
			{
				base.Renderer.Draw(this.Current);
			}
			#endregion

			public void ChangeWinner(bool b)
			{
				if (b)
				{
					this.Current = this.ImageBWon;
				}
				else
				{
					this.Current = this.ImageAWon;
				}
			}
		}
		#endregion
	}
}
