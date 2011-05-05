using System.Collections.Generic;
using Kingdoms_Clash.NET.Interfaces;
using ClashEngine.NET.Interfaces.EntitiesManager;
using ClashEngine.NET.EntitiesManager;

namespace Kingdoms_Clash.NET.Tests.TestObjects
{
	/// <summary>
	/// Stan gry do testów.
	/// </summary>
	public class GameState
		: IGameState
	{
		public static readonly Interfaces.Units.IUnitDescription TestUnit = new Units.UnitDescription("Unit1", 10, 10, 10, 10);

		public List<Interfaces.Units.IUnit> Units { get; private set; }
		public List<Interfaces.Map.IResourceOnMap> Resources { get; private set; }

		private IEntitiesManager Entities;

		#region IGameState Members
		public Interfaces.IGameplaySettings Settings { get; private set; }
		public Interfaces.Player.IPlayer[] Players { get; private set; }

		public Interfaces.Map.IMap Map { get; private set; }

		public Interfaces.Controllers.IGameController Controller { get; set; }

		public void Initialize(Interfaces.ISingleplayerSettings settings)
		{ }

		public void Reset()
		{ }

		public void Add(Interfaces.Units.IUnit unit)
		{
			this.Entities.Add(unit);
			this.Units.Add(unit);
		}

		public void Add(Interfaces.Map.IResourceOnMap resource)
		{
			this.Resources.Add(resource);
		}

		public void Kill(Interfaces.Units.IUnit unit)
		{
			this.Entities.Remove(unit);
			this.Units.Remove(unit);
		}

		public void Gather(Interfaces.Map.IResourceOnMap resource, Interfaces.Units.IUnit by)
		{
			this.Resources.Remove(resource);
		}
		#endregion

		public GameState()
		{
			this.Players = new Interfaces.Player.IPlayer[]
			{
				new Player.Player("A", new Units.Nation("TestNation", "", new Interfaces.Units.IUnitDescription[] { TestUnit }), 100),
				new Player.Player("B", new Units.Nation("TestNation", "", new Interfaces.Units.IUnitDescription[] { TestUnit }), 100)
			};

			this.Entities = new EntitiesManager(null);
			this.Map = new Maps.DefaultMap();
			this.Units = new List<Interfaces.Units.IUnit>();
			this.Resources = new List<Interfaces.Map.IResourceOnMap>();
		}
	}
}
