using System;

namespace Kingdoms_Clash.NET.Controllers.Internals
{
	using Interfaces.Player;
	using Interfaces.Units;

	internal class UnitRequestToken
		: IUnitRequestToken
	{
		#region Private fields
		private float _TimeLeft = 0f;
		#endregion

		#region IUnitRequestToken Members
		/// <summary>
		/// Czas, w sekundach, do stworzenia jednostki.
		/// </summary>
		public float TimeLeft
		{
			get { return this._TimeLeft; }
			internal set
			{
				this._TimeLeft = value;
				if (this._TimeLeft <= 0f)
				{
					this._TimeLeft = 0f;
					this.IsCompleted = true;
					//if (this.UnitCreated != null)
					//{
					//    this.UnitCreated(this, null);
					//}
				}
			}
		}

		/// <summary>
		/// Procent ukończenia jednostki.
		/// </summary>
		public float Percentage
		{
			get
			{
				return (this.Unit.CreationTime - this.TimeLeft) * this.Unit.CreationTime;
			}
		}

		/// <summary>
		/// Czy token jest poprawny i nadal można go używać.
		/// </summary>
		public bool IsValidToken { get; internal set; }

		/// <summary>
		/// Czy ukończono już jednostkę.
		/// </summary>
		public bool IsCompleted { get; private set; }

		/// <summary>
		/// Czy tworzenie jednostki jest spauzowane.
		/// </summary>
		public bool IsPaused { get; private set; }

		/// <summary>
		/// Jednostka, która zostanie stworzona.
		/// </summary>
		public IUnitDescription Unit { get; private set; }

		/// <summary>
		/// Właściciel jednostki.
		/// </summary>
		public IPlayer Owner { get; private set; }

		/// <summary>
		/// Zdarzenie wywoływane przy stworzeniu jednostki.
		/// </summary>
		public event EventHandler UnitCreated;

		/// <summary>
		/// Tworzy jednostkę z tokenu.
		/// </summary>
		/// <returns>Nowoutworzona jednostka.</returns>
		public IUnit CreateUnit()
		{
			if (this.IsValidToken)
			{
				return new Units.Unit(this.Unit, this.Owner);
			}
			return null;
		}
		#endregion

		#region Constructors
		public UnitRequestToken(IUnitDescription unit, IPlayer owner, bool start)
		{
			if (unit == null)
			{
				throw new ArgumentNullException("unit");
			}
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.Unit = unit;
			this.Owner = owner;
			this.IsPaused = !start;
			this.IsValidToken = true;
			this.IsCompleted = false;

			this._TimeLeft = this.Unit.CreationTime;
		}
		#endregion
	}
}
