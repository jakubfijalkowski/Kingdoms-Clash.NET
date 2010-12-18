using System.ComponentModel;
using ClashEngine.NET.Graphics.Gui;

namespace Kingdoms_Clash.NET.Player.Controllers.XAML
{
	using Interfaces.Controllers;
	using Interfaces.Units;

	/// <summary>
	/// Kontener GUI dla kontrolera gracza.
	/// </summary>
	public class PlayersGUIContainer
		: XamlGuiContainer, INotifyPropertyChanged
	{
		#region Private fields
		private Interfaces.Player.IPlayer _Player1;
		private Interfaces.Player.IPlayer _Player2;
		private IUnitQueue _Player1Queue;
		private IUnitQueue _Player2Queue;
		#endregion

		#region Players
		/// <summary>
		/// Pierwszy gracz.
		/// </summary>
		public Interfaces.Player.IPlayer Player1
		{
			get { return this._Player1; }
			internal set
			{
				this._Player1 = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Player1"));
				}
			}
		}

		/// <summary>
		/// Drugi gracz.
		/// </summary>
		public Interfaces.Player.IPlayer Player2
		{
			get { return this._Player2; }
			internal set
			{
				this._Player2 = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Player2"));
				}
			}
		}

		/// <summary>
		/// Kolejka jednostek pierwszego gracza.
		/// </summary>
		public IUnitQueue Player1Queue
		{
			get { return this._Player1Queue; }
			internal set
			{
				this._Player1Queue = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Player1Queue"));
				}
			}
		}

		/// <summary>
		/// Kolejka jednostek drugiego gracza.
		/// </summary>
		public IUnitQueue Player2Queue
		{
			get { return this._Player2Queue; }
			internal set
			{
				this._Player2Queue = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new PropertyChangedEventArgs("Player2Queue"));
				}
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Wywoływane przy zmianie Player1, Player1Queue, Player2 i Player2Queue.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Methods
		/// <summary>
		/// Do wywoływania z kodu XAML.
		/// Próbuje dodać jednostkę do kolejki.
		/// </summary>
		/// <param name="playerNo">Numer gracza(1 lub 2).</param>
		/// <param name="unit">Jednostka.</param>
		public void RequestUnit(int playerNo, IUnitDescription unit)
		{
			if (playerNo == 1)
			{
				this.Player1Queue.Request(unit.Id);
			}
			else
			{
				this.Player2Queue.Request(unit.Id);
			}
		}
		#endregion
	}
}
