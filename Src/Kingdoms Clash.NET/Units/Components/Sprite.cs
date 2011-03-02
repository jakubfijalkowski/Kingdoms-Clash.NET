using System.Diagnostics;
using ClashEngine.NET.Graphics.Resources;
using OpenTK;

namespace Kingdoms_Clash.NET.Units.Components
{
	using Interfaces.Units;
	using Interfaces.Units.Components;

	/// <summary>
	/// Opis komponentu dla jednostki posiadającej obrazek.
	/// </summary>
	[DebuggerDisplay("Sprite, Image = {ImagePath,nq}")]
	public class Sprite
		: ISprite
	{
		#region ISprite Members
		/// <summary>
		/// Ścieżka do obrazka
		/// </summary>
		public string ImagePath { get; set; }
		#endregion

		#region IUnitComponentDescription Members
		/// <summary>
		/// Tworzy komponent na podstawie opisu.
		/// </summary>
		/// <returns>Komponent.</returns>
		public IUnitComponent Create()
		{
			return new SpriteComponent() { Description = this };
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje opis.
		/// </summary>
		/// <param name="image">Ścieżka do obrazka.</param>
		public Sprite(string image)
		{
			this.ImagePath = image;
		}

		/// <summary>
		/// Inicjalizuje opis domyślnymi wartościami.
		/// </summary>
		public Sprite()
		{
			this.ImagePath = string.Empty;
		}
		#endregion

		#region Components
#if !SERVER
		/// <summary>
		/// Klasa właściwego komponentu - nie musi być widoczna publicznie.
		/// </summary>
		private class SpriteComponent
			: ClashEngine.NET.Graphics.Components.Sprite, IUnitComponent
		{
			#region IUnitComponent Members
			/// <summary>
			/// Opis komponentu.
			/// </summary>
			public IUnitComponentDescription Description { get; set; }
			#endregion

			#region Sprite Members
			public override void OnInit()
			{
				base.Texture = this.Content.Load<Texture>((this.Description as ISprite).ImagePath);
				base.OnInit();

				base.Size = new Vector2(
					(this.Owner as IUnit).Description.Width,
					(this.Owner as IUnit).Description.Height);
				
				if ((this.Owner as IUnit).Owner.Type == Interfaces.Player.PlayerType.First)
				{
					base.Effect = ClashEngine.NET.Interfaces.Graphics.Objects.SpriteEffect.FlipHorizontally;
				}
			}
			#endregion

			public SpriteComponent()
				: base("UnitSprite")
			{ }
		}
#else
		/// <summary>
		/// Klasa właściwego komponentu - nie musi być widoczna publicznie.
		/// Zaślepka - grafika nie jest używana w serwerze.
		/// </summary>
		private class SpriteComponent
			: ClashEngine.NET.EntitiesManager.Component, IUnitComponent
		{
			#region IUnitComponent Members
			/// <summary>
			/// Opis komponentu.
			/// </summary>
			public IUnitComponentDescription Description { get; set; }
			#endregion

			#region Component Members
			public override void Update(double delta)
			{ }
			#endregion

			public SpriteComponent()
				: base("UnitSprite")
			{ }
		}
#endif
		#endregion
	}
}
