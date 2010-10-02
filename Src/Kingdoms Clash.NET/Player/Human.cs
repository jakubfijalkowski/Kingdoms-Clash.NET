using System;
using System.Collections.Generic;
using ClashEngine.NET.EntitiesManager;
using ClashEngine.NET.Interfaces.EntitiesManager;

namespace Kingdoms_Clash.NET.Player
{
	using Interfaces.Player;
	using Interfaces.Resources;
	using Interfaces.Units;

	public class Human
		: GameEntity, IHuman
	{
		private IAttribute<int> Health_;

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
		#endregion

		/// <summary>
		/// Inicjalizuje nowego gracza.
		/// </summary>
		/// <param name="name">Nazwa.</param>
		/// <param name="nation">Jego nacja.</param>
		public Human(string name, INation nation/*, IEnumerable<IResource> startResources*/)
			: base("Player.Human." + name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}
			else if (nation == null)
			{
				throw new ArgumentNullException("nation");
			}
			this.Name = name;
			this.Nation = nation;
		}

		public override void OnInit()
		{
			this.Health_ = this.Attributes.GetOrCreate<int>("Health");
		}
	}
}
