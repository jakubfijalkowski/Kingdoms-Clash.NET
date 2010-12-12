using System;

namespace Kingdoms_Clash.NET.Controllers
{
	using Interfaces;
	using Interfaces.Controllers;
	using Interfaces.Map;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Klasyczna gra, zgodna z zasadami oryginału.
	/// </summary>
	public class ClassicGame
		: IClassicGame
	{
		#region Static fields
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		private static Random Random = new Random();
		#endregion

		#region IGameController Members
		/// <summary>
		/// Stan aktualnej gry.
		/// </summary>
		public IGameState GameState { get; set; }

		/// <summary>
		/// Kolejka produkcji jednostek dla pierwszego gracza.
		/// </summary>
		public IUnitQueue Player1Queue { get; private set; }

		/// <summary>
		/// Kolejka produkcji jednostek dla drugiego gracza.
		/// </summary>
		public IUnitQueue Player2Queue { get; private set; }

		/// <summary>
		/// Pobiera kolejkę jednostek dla wskazanego gracza.
		/// </summary>
		/// <param name="player">Gracz.</param>
		/// <returns></returns>
		public IUnitQueue this[IPlayer player]
		{
			get
			{
				if (player == this.GameState.Players[0])
				{
					return this.Player1Queue;
				}
				return this.Player2Queue;
			}
		}

		/// <summary>
		/// Dodaje nową jednostke.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public IUnitRequestToken RequestNewUnit(string id, IPlayer player)
		{
			if (this.GameState.Players[0] == player)
			{
				return this.Player1Queue.Request(id);
			}
			else
			{
				return this.Player2Queue.Request(id);
			}
		}

		/// <summary>
		/// Dodaje zasób o wskazanym Id i wartości pobranej z konfiguracji.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IResourceOnMap RequestNewResource(string id)
		{
			return new Maps.ResourceOnMap(id, Configuration.Instance.ResourceRenewalValue,
				Configuration.Instance.CastleSize.X + (float)(Random.NextDouble() * (this.GameState.Map.Size.X - Configuration.Instance.CastleSize.X * 2f)));
		}

		/// <summary>
		/// </summary>
		/// <param name="delta"></param>
		public void Update(double delta)
		{
			var u1 = this.Player1Queue.Update(delta);
			var u2 = this.Player2Queue.Update(delta);
			if (u1 != null)
			{
				this.HandleUnit(u1);
			}
			if (u2 != null)
			{
				this.HandleUnit(u2);
			}
		}

		/// <summary>
		/// Usuwamy wszystko, co było dodane i wraca do ustawień początkowych.
		/// </summary>
		public void Reset()
		{
			this.Player1Queue.Clear();
			this.Player2Queue.Clear();
			this.SetDefaults();
		}

		#region Events
		/// <summary>
		/// Obsługuje kolizje jednostki z zamkiem.
		/// Jeśli zamek jest przeciwnika - uszkadza go.
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="player"></param>
		public void HandleCollision(IUnit unit, IPlayer player)
		{
			if (unit.Owner != player)
			{
				var component = unit.Description.Components.GetSingle<Interfaces.Units.Components.IContactSoldier>();
				if (component != null)
				{
					player.Health -= component.Strength;
				}
			}
			this.GameState.Remove(unit);
			unit.Owner.Units.Remove(unit);
		}

		/// <summary>
		/// Obsługuje kolizje jednostki z jednostką.
		/// </summary>
		/// <param name="unitA"></param>
		/// <param name="unitB"></param>
		public void HandleCollision(IUnit unitA, IUnit unitB)
		{
			//Wygrywa silniejsza, a gdy równe - giną obie.
			int aStrength = 0;
			int bStrength = 0;
			var component = unitA.Description.Components.GetSingle<Interfaces.Units.Components.IContactSoldier>();
			if (component != null)
			{
				aStrength = component.Strength;
			}
			component = unitB.Description.Components.GetSingle<Interfaces.Units.Components.IContactSoldier>();
			if (component != null)
			{
				bStrength = component.Strength;
			}

			if (aStrength == bStrength)
			{
				this.GameState.Remove(unitA);
				unitA.Owner.Units.Remove(unitA);
				this.GameState.Remove(unitB);
				unitB.Owner.Units.Remove(unitB);
			}
			else if (aStrength > bStrength)
			{
				this.GameState.Remove(unitB);
				unitB.Owner.Units.Remove(unitB);
			}
			else
			{
				this.GameState.Remove(unitA);
				unitA.Owner.Units.Remove(unitA);
			}
		}

		/// <summary>
		/// Metoda-zdarzenie odpowiedzialna za kolizje jednostki z zasobem na mapie.
		/// </summary>
		/// <param name="unit">Jednostka.</param>
		/// <param name="resource">Zasób.</param>
		public bool HandleCollision(IUnit unit, IResourceOnMap resource)
		{
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void OnGameStarted()
		{
			this.Player1Queue = new Internals.UnitQueue(this.GameState.Players[0]);
			this.Player2Queue = new Internals.UnitQueue(this.GameState.Players[1]);
			this.SetDefaults();
		}
		#endregion
		#endregion

		#region Private
		private void SetDefaults()
		{
			foreach (var player in this.GameState.Players)
			{
				player.Units.Clear();
				player.Health = (int)player.MaxHealth;
			}

			//Testowe, początkowe zasoby
			foreach (var res in Resources.ResourcesList.Instance)
			{
				this.GameState.Players[0].Resources.Add(res.Id, Configuration.Instance.StartResources);
				this.GameState.Players[1].Resources.Add(res.Id, Configuration.Instance.StartResources);
			}
		}

		private void HandleUnit(IUnit unit)
		{
			unit.Owner.Units.Add(unit);
			this.GameState.Add(unit);

			//TODO: napisać ładniejszą dedukcje gdzie umieścić jednostkę.
			if (unit.Owner == this.GameState.Players[0])
			{
				unit.Position = this.GameState.Map.FirstCastle + Configuration.Instance.CastleSize - new OpenTK.Vector2(0f, unit.Description.Height);
			}
			else
			{
				unit.Position = this.GameState.Map.SecondCastle + new OpenTK.Vector2(-unit.Description.Width, Configuration.Instance.CastleSize.Y - unit.Description.Height);
			}
			Logger.Info("Player {0} created unit {1}", unit.Owner.Name, unit.Description.Id);
		}
		#endregion
	}
}
