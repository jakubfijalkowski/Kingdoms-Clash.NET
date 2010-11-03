using ClashEngine.NET.Cameras;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using ClashEngine.NET.ScreensManager;
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
			: base("WinnerScreen", ClashEngine.NET.Interfaces.ScreensManager.ScreenType.Normal)
		{
			this.GameState = gameState;
		}
		#endregion

		#region Screen Members
		public override void OnInit()
		{
			base.OnInit();

			float aspect = ImageSize.X / ImageSize.Y;
			this.Entities.Add(new OrthoCamera(new System.Drawing.RectangleF(0f, 0f, 1f, 1f * aspect), new OpenTK.Vector2(1f, 1f * aspect), 0f, true));
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
			/// <summary>
			/// "Duszek".
			/// </summary>
			private Sprite Image = new Sprite("Image");
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

				this.Components.Add(this.Image);
				this.Image.Init(this.Content.Load<Texture>("PlayerAWon.png"));
				this.Image.Size = new Vector2(w, h);
				this.Image.Position = new Vector2((1f - w) / 2f, (1f * aspect - h) / 2f);
			}
			#endregion

			public void ChangeWinner(bool b)
			{
				if (b)
				{
					this.Image.Init(this.Content.Load<Texture>("PlayerBWon.png"));
				}
				else
				{
					this.Image.Init(this.Content.Load<Texture>("PlayerAWon.png"));
				}
			}
		}
		#endregion
	}
}
