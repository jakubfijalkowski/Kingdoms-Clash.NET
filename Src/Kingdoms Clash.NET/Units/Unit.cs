using System;
using System.Diagnostics;
using ClashEngine.NET.Components;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.Utilities;
using FarseerPhysics.Dynamics;
using OpenTK;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Map;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Jednostka.
	/// </summary>
	[DebuggerDisplay("{Description.Id,nq}, Player = {Owner.Name,nq}, Health = {Health}")]
	public class Unit
		: GameEntity, IUnit
	{
		private IAttribute<int> Health_;
		private IAttribute<Vector2> Position_;

		#region IUnit Members
		/// <summary>
		/// Opis(typ/identyfikator) jednostki.
		/// </summary>
		public IUnitDescription Description { get; private set; }

		/// <summary>
		/// Właściciel jednostki.
		/// </summary>
		public IPlayer Owner { get; private set; }

		/// <summary>
		/// Życie.
		/// </summary>
		public int Health
		{
			get { return this.Health_.Value; }
			set { this.Health_.Value = value; }
		}

		/// <summary>
		/// Pozycja jednostki.
		/// </summary>
		public Vector2 Position
		{
			get { return this.Position_.Value; }
			set { this.Position_.Value = value; }
		}

		#region Events
		/// <summary>
		/// Zdarzenie kolizji jednostek.
		/// </summary>
		/// <seealso cref="CollisionWithUnitEventHandler"/>
		public event CollisionWithUnitEventHandler CollisionWithUnit;

		/// <summary>
		/// Zdarzenie kolizji jednostki z graczem.
		/// </summary>
		/// <seealso cref="CollisionWithPlayerEventHandler"/>
		public event CollisionWithPlayerEventHandler CollisionWithPlayer;

		/// <summary>
		/// Zdarzenie kolizji jednostki z zasobem na mapie..
		/// </summary>
		/// <seealso cref="CollisionWithResourceEventHandler"/>
		public event CollisionWithResourceEventHandler CollisionWithResource;
		#endregion
		#endregion

		/// <summary>
		/// Inicjalizuje nową jednostkę.
		/// </summary>
		/// <param name="description">Opis.</param>
		/// <param name="owner">Właściciel.</param>
		public Unit(IUnitDescription description, IPlayer owner)
			: base(string.Format("Unit.{0}.{1}", owner.Name, description.Id))
		{
			if (description == null)
			{
				throw new ArgumentNullException("description");
			}
			else if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.Description = description;
			this.Owner = owner;
		}

		public override void OnInit()
		{
			//Najpierw komponenty fizyczne, by dało się pobrać prawidłowy atrybut prędkości.
			var pObj = new PhysicalObject(true);
			this.Components.Add(pObj);
			this.Components.Add(new BoundingBox(new OpenTK.Vector2(this.Description.Width, this.Description.Height)));

			this.Health_ = this.Attributes.GetOrCreate<int>("Health");
			this.Position_ = this.Attributes.GetOrCreate<Vector2>("Position");

			this.Health = (int)this.Description.Health;

			//Ustawiamy właściwości ciała tak, by poruszało się po naszej myśli
			pObj.Body.SleepingAllowed = false;
			pObj.Body.FixedRotation = true;

			pObj.Body.UserData = this;

			//Ustawiamy maskę kolizji tak by kolidowało tylko z innymi graczami
			pObj.Body.SetCollisionCategories((CollisionCategory)((int)CollisionCategory.Cat1 << (int)this.Owner.Type));

			//Koliduje ze wszystkim z wyłączeniem jednostek tego samego gracza i zasobami.
			pObj.Body.SetCollidesWith(CollisionCategory.All & ~pObj.Body.GetCollisionCategories() &
				~CollisionCategory.Cat10 & ~((CollisionCategory)((int)CollisionCategory.Cat11 << (int)this.Owner.Type)));

			//I zdarzenia kolizji pomiędzy jednostkami
			pObj.Body.SetCollisionEvent((fixtureA, fixtureB, contact) =>
			{
				if (fixtureB.Body.UserData is IUnit && this.CollisionWithUnit != null)
				{
					this.CollisionWithUnit(this, fixtureB.Body.UserData as IUnit);
					return false;
				}
				else if (fixtureB.Body.UserData is IPlayer && this.CollisionWithPlayer != null)
				{
					this.CollisionWithPlayer(this, fixtureB.Body.UserData as IPlayer);
				}
				else if (fixtureB.Body.UserData is IResourceOnMap && this.CollisionWithResource != null)
				{
					//Taka iteracja zapobiega zbieraniu już zebranych zasobów
					var delegates = this.CollisionWithResource.GetInvocationList();
					foreach (CollisionWithResourceEventHandler d in delegates)
					{
						if (d(this, fixtureB.Body.UserData as IResourceOnMap))
						{
							(fixtureB.Body.UserData as IResourceOnMap).Gather();
							break;
						}
					}
				}
				return true;
			});

			foreach (var component in this.Description.Components)
			{
				this.Components.Add(component.Create());
			}
		}
	}
}
