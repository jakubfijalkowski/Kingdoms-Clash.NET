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
	[ControllerSettings(typeof(DefaultGameplaySettings))]
	public class ClassicGame
		: IClassicGame
	{
		#region Static fields
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");
		private static Random Random = new Random();
		#endregion

		#region Private fields
		/// <summary>
		/// Zmienna pomocnicza do określania kiedy dodać zasób.
		/// </summary>
		private float ResourceRenewalAccumulator = 0.0f;
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

			this.HandleResources(delta);
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
		/// Wywoływane na początku inicjalizacji, przy rozpoczynaniu gry.
		/// </summary>
		public void PreGameStarted()
		{
			this.Player1Queue = new Internals.UnitQueue(this.GameState.Players[0]);
			this.Player2Queue = new Internals.UnitQueue(this.GameState.Players[1]);
		}

		/// <summary>
		/// 
		/// </summary>
		public void GameStarted()
		{
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
				this.GameState.Players[0].Resources.Add(res.Id, this.GameState.Settings.Gameplay.StartResources);
				this.GameState.Players[1].Resources.Add(res.Id, this.GameState.Settings.Gameplay.StartResources);
			}

			this.ResourceRenewalAccumulator = 0.0f;
		}

		private void HandleUnit(IUnit unit)
		{
			unit.Owner.Units.Add(unit);
			this.GameState.Add(unit);

			//TODO: napisać ładniejszą dedukcje gdzie umieścić jednostkę.
			if (unit.Owner == this.GameState.Players[0])
			{
				unit.Position = this.GameState.Map.FirstCastle + Settings.CastleSize - new OpenTK.Vector2(-1f, unit.Description.Height);
			}
			else
			{
				unit.Position = this.GameState.Map.SecondCastle + new OpenTK.Vector2(-unit.Description.Width - 1f, Settings.CastleSize.Y - unit.Description.Height);
			}
			Logger.Info("Player {0} created unit {1}", unit.Owner.Name, unit.Description.Id);
		}

		/// <summary>
		/// Obsługa zasobów.
		/// Obsługiwane zasoby: wood.
		/// </summary>
		/// <param name="delta"></param>
		private void HandleResources(double delta)
		{
			this.ResourceRenewalAccumulator += (float)delta;
			if (this.ResourceRenewalAccumulator > this.GameState.Settings.Gameplay.ResourceRenewalTime)
			{
				var res = new Maps.ResourceOnMap("wood", this.GameState.Settings.Gameplay.ResourceRenewalValue,
				Settings.CastleSize.X + (float)(Random.NextDouble() * (this.GameState.Map.Size.X - Settings.CastleSize.X * 2f)));
				if (res != null)
				{
					this.GameState.Add(res);
				}
				this.ResourceRenewalAccumulator -= this.GameState.Settings.Gameplay.ResourceRenewalTime;
			}
		}
		#endregion
	}
}
