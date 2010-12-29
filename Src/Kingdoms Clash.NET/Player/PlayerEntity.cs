using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Extensions;
using ClashEngine.NET.Graphics.Components;
using ClashEngine.NET.Graphics.Resources;
using OpenTK;

namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;

	/// <summary>
	/// Encja gracza - jego zamek.
	/// </summary>
	public class PlayerEntity
		: GameEntity, IPlayerEntity
	{
		#region IPlayerEntity Members
		/// <summary>
		/// Gracz do którego się odnosi.
		/// </summary>
		public IPlayer Player { get; private set; }

		/// <summary>
		/// Stan gry dla gracza.
		/// </summary>
		public Interfaces.IGameState GameState { get; private set; }
		#endregion

		#region GameEntity Members
		public override void OnInit()
		{
			//Tworzymy zamek.
			var pObj = new PhysicalObject();
			this.Components.Add(pObj);
			this.Components.Add(new BoundingBox(Configuration.Instance.CastleSize));
			pObj.Body.SetCollisionCategories(FarseerPhysics.Dynamics.CollisionCategory.Cat20);
			pObj.Body.UserData = this;

			Sprite s = new Sprite("CastleImage", this.GameInfo.Content.Load<Texture>(this.Player.Nation.CastleImage));
			this.Components.Add(s);
			if (this.Player.Type == PlayerType.Second)
			{
				s.Effect = ClashEngine.NET.Interfaces.Graphics.Objects.SpriteEffect.FlipHorizontally;
				this.Attributes.Get<Vector2>("Position").Value = this.GameState.Map.SecondCastle;
			}
			else
			{
				this.Attributes.Get<Vector2>("Position").Value = this.GameState.Map.FirstCastle;
			}

			this.Attributes.Get<Vector2>("Size").Value = Configuration.Instance.CastleSize;
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje encję.
		/// </summary>
		/// <param name="player"></param>
		public PlayerEntity(IPlayer player, Interfaces.IGameState state)
			: base("Player." + player.Name)
		{
			this.Player = player;
			this.GameState = state;
		}
		#endregion
	}
}
