using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Player;
	using Interfaces.Units;
	
	/// <summary>
	/// Jednostka.
	/// 
	/// Jest tworzona na podstawie opisu.
	/// </summary>
	public class Unit
		: GameEntity, IUnit
	{
		private IAttribute<int> Health_ = null;

		#region IUnit Members
		/// <summary>
		/// Opis jednostki.
		/// </summary>
		public IUnitDescription Description { get; private set; }

		/// <summary>
		/// Właściciel.
		/// </summary>
		public IPlayer Owner { get; private set; }

		/// <summary>
		/// Zdrowie jednostki.
		/// </summary>
		public int Health
		{
			get { return this.Health_.Value; }
			set { this.Health_.Value = value; }
		}
		#endregion

		/// <summary>
		/// Konstruuje jednostkę na podstawie jej opisu.
		/// </summary>
		/// <param name="description">Opis.</param>
		/// <param name="owner">Właściciel.</param>
		public Unit(IUnitDescription description, IPlayer owner)
			: base(description.Id)
		{
			this.Health_ = this.GetOrCreateAttribute<int>("Health");

			this.Description = description;
			this.Owner = owner;
		}

		/// <summary>
		/// Inicjalizuje jednostkę - ustawia wszystkie parametry.
		/// </summary>
		public override void InitEntity()
		{
			this.Health_.Value = this.Description.Health;
			foreach (var component in this.Description.Components)
			{
				this.AddComponent(component);
			}
		}
	}
}
