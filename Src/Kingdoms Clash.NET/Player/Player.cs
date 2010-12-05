using System;
using System.Collections.Generic;

namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;
	using Interfaces.Resources;
	using Interfaces.Units;

	/// <summary>
	/// Gracz.
	/// </summary>
	public class Player
		: IPlayer
	{
		#region Private fields
		private int _Health = 0;
		#endregion

		#region IPlayer Members
		/// <summary>
		/// Nazwa gracza.
		/// </summary>
		public string Name { get; private set; }

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
			get { return this._Health; }
			set
			{
				this._Health = value;
				if (this._Health > this.MaxHealth)
				{
					this._Health = (int)this.MaxHealth;
				}
			}
		}

		/// <summary>
		/// Maksymalna ilość zdrowia.
		/// </summary>
		public uint MaxHealth { get; private set; }

		/// <summary>
		/// Typ gracza.
		/// </summary>
		public PlayerType Type { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Inicjalizuje.
		/// </summary>
		/// <param name="id">Nazwa gracza</param>
		/// <param name="name">Nazwa.</param>
		/// <param name="nation">Jego nacja.</param>
		public Player(string name, INation nation, uint maxHealth)
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
			this.MaxHealth = maxHealth;
		}
		#endregion
	}
}
