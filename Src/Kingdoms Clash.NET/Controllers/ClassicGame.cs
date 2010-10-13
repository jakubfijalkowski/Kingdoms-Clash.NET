using System;

namespace Kingdoms_Clash.NET.Controllers
{
	using Interfaces;
	using Interfaces.Map;
	using Interfaces.Controllers;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Klasyczna gra, zgodna z zasadami oryginału.
	/// </summary>
	public class ClassicGame
		: IGameController
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region IGameController Members
		/// <summary>
		/// Stan aktualnej gry.
		/// </summary>
		public IGameState GameState { get; set; }

		/// <summary>
		/// Dodaje nową jednostke.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool RequestNewUnit(string id, IPlayer player)
		{
			//Sprawdzamy, czy gracz ma wystarczającą ilość zasobów.
			var ud = player.Nation.AvailableUnits[id];
			if (ud != null)
			{
				foreach (var cost in ud.Costs)
				{
					if (!player.Resources.Contains(cost))
					{
						Logger.Debug("Insufficient resources({0}) to create unit {1}", cost.Key, ud.Id);
						return false;
					}
				}
				//Tak, posiadamy - czyli możemy je zmniejszyć
				foreach (var cost in ud.Costs)
				{
					player.Resources.Remove(cost);
				}
			}
			else
			{
				return false;
			}

			var unit = player.Nation.CreateUnit(id, player);
			if (unit != null)
			{
				player.Units.Add(unit);
				this.GameState.Add(unit);

				//TODO: napisać ładniejszą dedukcje gdzie umieścić jednostkę.
				if (player == this.GameState.Players[0])
				{
					unit.Position = this.GameState.Map.FirstCastle + Configuration.Instance.CastleSize - new OpenTK.Vector2(0f, unit.Description.Height);
				}
				else
				{
					unit.Position = this.GameState.Map.SecondCastle + new OpenTK.Vector2(-unit.Description.Width, Configuration.Instance.CastleSize.Y - unit.Description.Height);
				}
				Logger.Info("Player {0} created unit {1}", player.Nation, unit.Description.Id);
				return true;
			}
			Logger.Warn("Unit with id {0} not found in nation {1}", id, player.Nation.Name);
			return false;
		}

		/// <summary>
		/// Sprawdza, czy któryś z graczy nie wygrał.
		/// </summary>
		/// <param name="delta"></param>
		public void Update(double delta)
		{
			if (this.GameState.Players[0].Health <= 0)
			{
				Logger.Error("User {0} has won the match!", this.GameState.Players[1].Name);
				throw new NotSupportedException();
			}
			else if (this.GameState.Players[1].Health <= 0)
			{
				Logger.Error("User {0} has won the match!", this.GameState.Players[0].Name);
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Usuwamy wszystko, co było dodane i wraca do ustawień początkowych.
		/// </summary>
		public void Reset()
		{
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
			this.SetDefaults();
		}
		#endregion
		#endregion

		#region Private
		private void SetDefaults()
		{
			this.GameState.Players[0].Health = 100;
			this.GameState.Players[1].Health = 100;

			//Testowe, początkowe zasoby
			foreach (var res in Resources.ResourcesList.Instance)
			{
				this.GameState.Players[0].Resources.Add(res.Id, Configuration.Instance.StartResources);
				this.GameState.Players[1].Resources.Add(res.Id, Configuration.Instance.StartResources);
			}
		}
		#endregion
	}
}
