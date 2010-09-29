using System;
using ClashEngine.NET.Components.Physical;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.PhysicsManager;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Player;
	using Interfaces.Units;

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
		#endregion

		/// <summary>
		/// Inicjalizuje nową jednostkę.
		/// TODO: usunąć sprawdzenie owner == null
		/// </summary>
		/// <param name="description">Opis.</param>
		/// <param name="owner">Właściciel.</param>
		public Unit(IUnitDescription description, IPlayer owner)
			: base(string.Format("Unit.{0}.{1}", (owner == null ? "player" : owner.Name), description.Id))
		{
			if (description == null)
			{
				throw new ArgumentNullException("description");
			}
			//else if (owner == null)
			//{
			//    throw new ArgumentNullException("owner");
			//}
			this.Description = description;
			this.Owner = owner;
		}

		public override void InitEntity()
		{
			this.Health = this.Description.Health;

			this.Components.Add(new PhysicalObject(true));
			this.Components.Add(new BoundingBox(new OpenTK.Vector2(this.Description.Width, this.Description.Health)));

			foreach (var component in this.Description.Components)
			{
				this.Components.Add(component);
			}
		}
	}
}
