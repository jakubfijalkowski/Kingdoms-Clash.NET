using System;
using System.Collections.Generic;
using ClashEngine.NET.Components;
using ClashEngine.NET.Components.Physical;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.PhysicsManager;
using ClashEngine.NET.Resources;
using ClashEngine.NET.ResourcesManager;
using FarseerPhysics.Dynamics;
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
		public IList<IUnit> Units
		{
			get { throw new NotImplementedException(); }
		}


		/// <summary>
		/// Zasoby, które gracz aktualnie posiada.
		/// </summary>
		public IList<IResource> Resources
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Zdrowie bohatera(jego zamku).
		/// </summary>
		public int Health
		{
			get { return this.Health_.Value; }
			set { this.Health_.Value = value; }
		}

		/// <summary>
		/// Zdarzenie kolizji jednostki z graczem.
		/// </summary>
		/// <seealso cref="UnitCollideWithPlayerEventHandler"/>
		public event UnitCollideWithPlayerEventHandler Collide;

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
			this.Components.Add(new PhysicalObject());
			this.Components.Add(new BoundingBox(Configuration.Instance.CastleSize));
			this.Components.Add(new Sprite("CastleImage", ResourcesManager.Instance.Load<Texture>(this.Nation.CastleImage)));

			this.Attributes.Get<Vector2>("Position").Value = (this.Type == PlayerType.First ? this.GameState.Map.FirstCastle : this.GameState.Map.SecondCastle);
			this.Attributes.Get<Vector2>("Size").Value = Configuration.Instance.CastleSize;

			//Gdy zderzy się z jednostką to wysyła odpowiednie zdarzenie
			this.Attributes.Get<Body>("Body").Value.FixtureList[0].OnCollision = (fixtureA, fixtureB, contact) =>
				{
					if (fixtureA.UserData is IUnit && fixtureB.UserData is IPlayer && this.Collide != null)
					{
						this.Collide(fixtureA.UserData as IUnit, fixtureB.UserData as IPlayer);
						return true;	
					}
					else if (fixtureA.UserData is IPlayer && fixtureB.UserData is IUnit && this.Collide != null)
					{
						this.Collide(fixtureB.UserData as IUnit, fixtureA.UserData as IPlayer);
						return true;
					}
					return false;
				};
		}
	}
}
