using System;
using System.Collections.Generic;

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

		#region Private fields
		private List<Internals.UnitRequestToken> UnitTokens1 = new List<Internals.UnitRequestToken>();
		private List<Internals.UnitRequestToken> UnitTokens2 = new List<Internals.UnitRequestToken>();
		#endregion

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
		public IUnitRequestToken RequestNewUnit(string id, IPlayer player)
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
						return null;
					}
				}
				//Tak, posiadamy - czyli możemy je zmniejszyć
				foreach (var cost in ud.Costs)
				{
					player.Resources.Remove(cost);
				}

				//I możemy dodać token.
				var token = new Internals.UnitRequestToken(ud, player, true);
				if (this.GameState.Players[0] == player)
				{
					this.UnitTokens1.Add(token);
				}
				else
				{
					this.UnitTokens2.Add(token);
				}
				return token;
			}
			Logger.Warn("Unit with id {0} not found in nation {1}", id, player.Nation.Name);
			return null;
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
			this.HandleUnits(this.UnitTokens1, delta);
			this.HandleUnits(this.UnitTokens2, delta);
		}

		/// <summary>
		/// Usuwamy wszystko, co było dodane i wraca do ustawień początkowych.
		/// </summary>
		public void Reset()
		{
			foreach (var token in this.UnitTokens1)
			{
				token.IsValidToken = false;
			}
			foreach (var token in this.UnitTokens2)
			{
				token.IsValidToken = false;
			}
			this.UnitTokens1.Clear();
			this.UnitTokens2.Clear();
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

		private void HandleUnits(List<Internals.UnitRequestToken> tokens, double delta)
		{
			for (int i = 0; i < tokens.Count; i++)
			{
				if (!tokens[i].IsPaused)
				{
					tokens[i].TimeLeft -= (float)delta;

					//Jednostka została ukończona.
					if (tokens[i].IsCompleted)
					{
						//Dodajemy jednostkę
						var unit = tokens[i].CreateUnit();
						tokens[i].Owner.Units.Add(unit);
						this.GameState.Add(unit);

						//TODO: napisać ładniejszą dedukcje gdzie umieścić jednostkę.
						if (tokens[i].Owner == this.GameState.Players[0])
						{
							unit.Position = this.GameState.Map.FirstCastle + Configuration.Instance.CastleSize - new OpenTK.Vector2(0f, unit.Description.Height);
						}
						else
						{
							unit.Position = this.GameState.Map.SecondCastle + new OpenTK.Vector2(-unit.Description.Width, Configuration.Instance.CastleSize.Y - unit.Description.Height);
						}
						Logger.Info("Player {0} created unit {1}", tokens[i].Owner.Name, unit.Description.Id);

						//Niszczymy token
						tokens[i].IsValidToken = false;
						tokens.RemoveAt(i);
						--i;
					}
					break;
				}
			}
		}
		#endregion
	}
}
