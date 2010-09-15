using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Units
{
	using Interfaces.Player;
	using Interfaces.Resources;
	using Interfaces.Units;

	/// <summary>
	/// Pracownik-zbieracz.
	/// Tworzony jest na podstawie opisu.
	/// </summary>
	public class Worker
		: GameEntity, IWorker
	{
		private IResource Cargo_ = null;
		private IAttribute<int> Health_ = null;
		private IAttribute<int> Speed_ = null;
		private IAttribute<int> MaxCargoSize_ = null;

		#region IWorker Members
		/// <summary>
		/// Ładunek niesiony przez pracownika lub null, gdy nic nie niesie.
		/// </summary>
		public IResource Cargo
		{
			get { return this.Cargo_; }
			set
			{
				this.Cargo_ = value;
				if (this.Cargo_.Value > this.MaxCargoSize)
				{
					this.Cargo_.Value = this.MaxCargoSize;
				}
			}
		}

		/// <summary>
		/// Maksymalna rozmiar ładunku niesionego przez pracownika.
		/// </summary>
		public int MaxCargoSize
		{
			get { return this.MaxCargoSize_.Value; }
			private set { this.MaxCargoSize_.Value = value; }
		}
		#endregion

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
		
		/// <summary>
		/// Prędkość jednostki.
		/// </summary>
		public int Speed
		{
			get { return this.Speed_.Value; }
		}
		#endregion

		/// <summary>
		/// Konstruuje jednostkę-robotnika na podstawie jej opisu.
		/// </summary>
		/// <param name="description">Opis.</param>
		/// <param name="owner">Właściciel.</param>
		public Worker(IWorkerDescription description, IPlayer owner)
			: base(description.Id)
		{
			this.Health_ = this.GetOrCreateAttribute<int>("Health");
			this.Speed_ = this.GetOrCreateAttribute<int>("Speed");
			this.MaxCargoSize_ = this.GetOrCreateAttribute<int>("MaxCargoSize");

			this.Description = description;
			this.Owner = owner;
			this.MaxCargoSize = description.MaxCargoSize;
		}
	}
}
