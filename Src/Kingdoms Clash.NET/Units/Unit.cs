using System;
using System.Diagnostics;
using ClashEngine.NET.Components.Physical;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.PhysicsManager;
using FarseerPhysics.Dynamics;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Jednostka.
	/// </summary>
	[DebuggerDisplay("{Description.Id,nq}, Player = {Owner.Name,nq}, Health = {Health}")]
	public class Unit
		: GameEntity, IUnit
	{
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
		public int Health { get; set; }

		/// <summary>
		/// Zdarzenie kolizji jednostek.
		/// </summary>
		public event UnitCollideEventHandler Collide;
		#endregion

		/// <summary>
		/// Inicjalizuje nową jednostkę.
		/// TODO: usunąć sprawdzenie owner == null
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
			this.Health = this.Description.Health;

			this.Components.Add(new PhysicalObject(true));
			this.Components.Add(new BoundingBox(new OpenTK.Vector2(this.Description.Width, this.Description.Height)));

			//Ustawiamy właściwości ciała tak, by poruszało się po naszej myśli
			var body = this.Attributes.Get<Body>("Body").Value; //Musi istnieć - odpowiedni komponent został dodany
			body.SleepingAllowed = false;
			body.FixedRotation = true;

			body.UserData = this;

			//Ustawiamy maskę kolizji tak by kolidowało tylko z innymi graczami
			var f = body.FixtureList[0];
			f.CollisionCategories = (CollisionCategory)(1 << (int)this.Owner.Type);
			f.CollidesWith = CollisionCategory.All & ~f.CollisionCategories;

			//I zdarzenia kolizji pomiędzy jednostkami
			f.OnCollision = (fixtureA, fixtureB, contact) =>
			{
				if (this.Collide != null && fixtureA.Body.UserData is IUnit && fixtureB.Body.UserData is IUnit)
				{
					this.Collide(fixtureA.Body.UserData as IUnit, fixtureB.Body.UserData as IUnit);
					return false;
				}
				return true;
			};

			foreach (var component in this.Description.Components)
			{
				this.Components.Add(component);
			}
		}
	}
}
