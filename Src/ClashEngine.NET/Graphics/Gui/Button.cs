namespace ClashEngine.NET.Graphics.Gui
{
	using Interfaces.Graphics.Gui;
	using Utilities;

	/// <summary>
	/// Przycisk.
	/// </summary>
	public class Button
		: ControlBase, IButton
	{
		#region Private fields
		private bool WasActive = false;
		private bool IsActive = false;
		private bool _Clicked = false;
		#endregion
		
		#region IButton Members
		/// <summary>
		/// Rozmiar przycisku.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Converters.Vector2Converter))]
		public OpenTK.Vector2 Size { get; set; }

		/// <summary>
		/// Czy przycisk jest wciśnięty.
		/// </summary>
		public bool Clicked
		{
			get { return this._Clicked; }
			private set
			{
				this._Clicked = value;
				if (this.PropertyChanged != null)
				{
					this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Clicked"));
				}
			}
		}
		#endregion

		#region INotifyPropertyChanged Members
		/// <summary>
		/// Zdarzenie zmiany właściwości Clicked.
		/// </summary>
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region ControlBase Members
		/// <summary>
		/// Nie potrzebujemy być aktywnym dłuższy czas.
		/// </summary>
		public override bool PermanentActive
		{
			get { return false; }
		}

		/// <summary>
		/// Myszka jest nad kontrolką, jeśli jest w prostokącie składającym się z Position i Size.
		/// </summary>
		/// <returns></returns>
		public override bool ContainsMouse()
		{
			return base.Data.Input.MousePosition.IsIn(this.Position, this.Size);
		}

		/// <summary>
		/// Jeśli PUSZCZONO myszkę nad przyciskiem to ustawiamy Clicked na true.
		/// </summary>
		/// <param name="delta"></param>
		public override void Update(double delta)
		{
			this.WasActive = this.IsActive;
			this.IsActive = this.Data.Active == this;

			if (this.WasActive && this.Data.Active == null && this.Data.Hot == this)
			{
				this.Clicked = true;
			}
		}

		/// <summary>
		/// Jeśli jest kliknięty zwraca 1, w przeciwnym razie 0.
		/// </summary>
		/// <returns></returns>
		public override int Check()
		{
			if (this.Clicked)
			{
				this.Clicked = false;
				return 1;
			}
			return 0;
		}
		#endregion
	}
}
