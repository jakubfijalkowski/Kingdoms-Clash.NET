using System;
using System.Collections.Generic;
using ClashEngine.NET.Components;
using ClashEngine.NET.Components.Physical;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Graphics.Components;
using ClashEngine.NET.Graphics.Resources;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Utilities;
using OpenTK;

namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;
	using Interfaces.Resources;
	using Interfaces.Units;

	/// <summary>
	/// Klasa bazowa dla gracz posiadających zamek.
	/// </summary>
	public abstract class Castle
		: GameEntity, IPlayer
	{
		private IAttribute<int> Health_;

		#region IPlayer Members
		/// <summary>
		/// Nazwa gracza.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Stan gry do której należy gracz.
		/// </summary>
		public Interfaces.IGameState GameState { get; set; }

		/// <summary>
		/// Nacja gracza.
		/// </summary>
		public INation Nation { get; private set; }

		/// <summary>
		/// Jednostki(aktualnie przebywające na planszy) gracza.
		/// </summary>
		public IList<IUnit> Units { get; private set; }

		/// <summary>
		/// Zasoby, które gracz aktualnie posiada.
		/// </summary>
		public IResourcesCollection Resources { get; private set; }

		/// <summary>
		/// Zdrowie bohatera(jego zamku).
		/// </summary>
		public int Health
		{
			get { return this.Health_.Value; }
			set { this.Health_.Value = value; }
		}

		/// <summary>
		/// Typ gracza.
		/// </summary>
		public PlayerType Type { get; set; }
		#endregion

		/// <summary>
		/// Inicjalizuje.
		/// </summary>
		/// <param name="entityId">Nazwa encji.</param>
		/// <param name="id">Numeryczny identyfikator gracza.</param>
		/// <param name="name">Nazwa.</param>
		/// <param name="nation">Jego nacja.</param>
		public Castle(string entityId, string name, INation nation/*, IEnumerable<IResource> startResources*/)
			: base(entityId)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}
			if (nation == null)
			{
				throw new ArgumentNullException("nation");
			}

			this.Units = new List<IUnit>();
			this.Resources = new Resources.ResourcesCollection();

			this.Name = name;
			this.Nation = nation;
		}

		/// <summary>
		/// Przy wywoływaniu tej metody mamy już w pełni zainicjowanego gracza.
		/// </summary>
		public override void OnInit()
		{
			this.Health_ = this.Attributes.GetOrCreate<int>("Health");

			//Tworzymy zamek.
			var pObj = new PhysicalObject();
			this.Components.Add(pObj);
			this.Components.Add(new BoundingBox(Configuration.Instance.CastleSize));
			pObj.Body.SetCollisionCategories(FarseerPhysics.Dynamics.CollisionCategory.Cat20);
			pObj.Body.UserData = this;

			Sprite s = new Sprite("CastleImage", this.Content.Load<Texture>(this.Nation.CastleImage));
			this.Components.Add(s);
			if (this.Type == PlayerType.Second)
			{
				s.Effect = ClashEngine.NET.Interfaces.Graphics.Objects.SpriteEffect.FlipHorizontally;
			}

			this.Attributes.Get<Vector2>("Position").Value = (this.Type == PlayerType.First ? this.GameState.Map.FirstCastle : this.GameState.Map.SecondCastle);
			this.Attributes.Get<Vector2>("Size").Value = Configuration.Instance.CastleSize;
		}
	}
}
