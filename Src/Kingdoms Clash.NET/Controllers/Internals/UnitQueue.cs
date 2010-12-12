using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Kingdoms_Clash.NET.Controllers.Internals
{
	using Interfaces.Controllers;
	using Interfaces.Player;
	using Interfaces.Units;

	/// <summary>
	/// Implementacja kolejki produkcji jednostek.
	/// Dla gry "klasycznej".
	/// </summary>
	internal class UnitQueue
		: IUnitQueue
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("KingdomsClash.NET");

		#region Private fields
		private List<UnitRequestToken> Queue = new List<UnitRequestToken>();
		#endregion

		#region IUnitQueue Members
		/// <summary>
		/// Gracz, do któreo dana kolejka należy.
		/// </summary>
		public IPlayer Player { get; private set; }

		/// <summary>
		/// Całkowita ługość kolejki.
		/// </summary>
		public uint QueueLength { get { return (uint)this.Queue.Count; } }

		/// <summary>
		/// Możemy produkować na raz tylko jedna jednostkę danego typu.
		/// </summary>
		public uint MaxProducedUnitsOfOneType { get { return 1; } }

		/// <summary>
		/// Możemy produkować tylko jedną jednostkę w ogóle.
		/// </summary>
		public uint MaxProducedUnits { get { return 1; } }

		/// <summary>
		/// Pobiera dłogość kolejki produkcyjnej danej jednostki.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <returns></returns>
		public uint this[string id]
		{
			get
			{
				var unit = this.Player.Nation.AvailableUnits[id];
				if (unit == null)
				{
					return 0;
				}
				return (uint)this.Queue.Count(t => t.Unit == unit);
			}
		}

		/// <summary>
		/// Uaktualnia kolejkę.
		/// </summary>
		/// <param name="delta">Czas od ostatniego uaktualnienia.</param>
		/// <returns>Jeśli jakaś jednostka została utworzona to zwraca token, w przeciwnym razie zwraca null.</returns>
		public IUnit Update(double delta)
		{
			if (this.Queue.Count > 0)
			{
				for (int i = 0; i < this.Queue.Count; i++)
				{
					if (!this.Queue[i].IsPaused)
					{
						this.Queue[i].TimeLeft -= (float)delta;
						if (this.Queue[i].IsCompleted)
						{
							var token = this.Queue[i];
							this.Queue.RemoveAt(i);
							if (this.PropertyChanged != null)
							{
								this.PropertyChanged(this, new PropertyChangedEventArgs("Item." + token.Unit.Id));
								this.PropertyChanged(this, new PropertyChangedEventArgs("QueueLength"));
							}
							var unit = token.CreateUnit();
							token.IsValidToken = false;
							return unit;
						}
						break;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Prosi o dodanie jednostki do kolejki.
		/// </summary>
		/// <param name="id">Identyfikator jednostki.</param>
		/// <returns>Token lub null, gdy nie dało się utworzyć jednostki.</returns>
		public IUnitRequestToken Request(string id)
		{
			//Sprawdzamy, czy gracz ma wystarczającą ilość zasobów.
			var ud = this.Player.Nation.AvailableUnits[id];
			if (ud != null)
			{
				foreach (var cost in ud.Costs)
				{
					if (!this.Player.Resources.Contains(cost))
					{
						Logger.Debug("Insufficient resources({0}) to create unit {1}", cost.Key, ud.Id);
						return null;
					}
				}
				//Tak, posiadamy - czyli możemy je zmniejszyć
				foreach (var cost in ud.Costs)
				{
					this.Player.Resources.Remove(cost);
				}

				//I możemy dodać token.
				var token = new Internals.UnitRequestToken(ud, this.Player, true);
				this.Queue.Add(token);
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Item." + token.Unit.Id));
					this.PropertyChanged(this, new PropertyChangedEventArgs("QueueLength"));
				}
				return token;
			}
			Logger.Warn("Unit with id {0} not found in nation {1}", id, this.Player.Nation.Name);
			return null;
		}
		/// <summary>
		/// Czyści kolejkę.
		/// </summary>
		public void Clear()
		{
			foreach (var item in this.Queue)
			{
				item.IsValidToken = false;
			}
			this.Queue.Clear();
		}
		#endregion

		#region Constructors
		public UnitQueue(IPlayer player)
		{
			this.Player = player;
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy dodaniu elementu do kolejki lub, gdy jednostka zostanie ukończona.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}
